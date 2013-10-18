using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MapEditor.D3D
{
    internal class RenderManager
    {
        public static RenderManager Instance = new RenderManager();

        public GxDevice Device { get; private set; }
        public RenderDispatcher Dispatcher { get; private set; }

        private int mFrameCount = 0;
        private DateTime mLastFrameUpdate = DateTime.Now;
        private DispatcherTimer mTimer;

        private List<WeakReference<IVideoResource>> mResources = new List<WeakReference<IVideoResource>>();

        public void init()
        {
            Dispatcher = new RenderDispatcher();
            Dispatcher.MainThread = System.Threading.Thread.CurrentThread;

            Device = new GxDevice();
            Device.init(MainWindow.Instance.RenderHost);

            Device.DeviceLost += onDeviceLost;
            Device.DeviceReset += onDeviceReset;

            mLastFrameUpdate = DateTime.Now;

            mTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(10), DispatcherPriority.ApplicationIdle, onIdle, MainWindow.Instance.Dispatcher);
            mTimer.Start();
        }

        private void onIdle(object sender, EventArgs args)
        {
            ++mFrameCount;
            var diff = DateTime.Now - mLastFrameUpdate;
            if(diff.TotalSeconds >= 1.0)
            {
                double fps = mFrameCount / diff.TotalSeconds;
                mFrameCount = 0;
                MainWindow.Instance.Title = "MapEditor - FPS: " + fps.ToString("F2");
                mLastFrameUpdate = DateTime.Now;
            }

            Device.onFrame();

            Dispatcher.runFrame();
        }

        private void onDeviceLost()
        {
            List<IVideoResource> removeResources = new List<IVideoResource>();
            IVideoResource tmpRes = null;

            mResources.RemoveAll(wr => wr.TryGetTarget(out tmpRes) == false);
            
            foreach(var rf in mResources)
            {
                rf.TryGetTarget(out tmpRes);
                if (tmpRes != null)
                    tmpRes.onDeviceLost();
            }
        }

        public void onDeviceReset()
        {
            List<IVideoResource> removeResources = new List<IVideoResource>();
            IVideoResource tmpRes = null;

            mResources.RemoveAll(wr => wr.TryGetTarget(out tmpRes) == false);

            foreach (var rf in mResources)
            {
                rf.TryGetTarget(out tmpRes);
                if (tmpRes != null)
                    tmpRes.onDeviceReset();
            }
        }

        public T getResource<T>() where T : IVideoResource, new()
        {
            T ret = new T();
            mResources.Add(new WeakReference<IVideoResource>(ret));
            return ret;
        }
    }
}
