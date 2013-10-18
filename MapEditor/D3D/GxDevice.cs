using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SlimDX.Direct3D9;

namespace MapEditor.D3D
{
    internal class GxDevice
    {
        private Control mControl;
        private Device mDevice;
        private Direct3D mDriver = new Direct3D();
        private PresentParameters mParams;

        public event Action DeviceLost;
        public event Action DeviceReset;

        internal Device Device { get { return mDevice; } }

        public GxDevice()
        {

        }

        public void init(Control ctrl)
        {
            mControl = ctrl;

            mParams = new PresentParameters()
            {
                AutoDepthStencilFormat = Format.D24S8,
                BackBufferFormat = Format.X8R8G8B8,
                BackBufferHeight = mControl.ClientSize.Height,
                BackBufferWidth = mControl.ClientSize.Width,
                DeviceWindowHandle = mControl.Handle,
                EnableAutoDepthStencil = true,
                Multisample = MultisampleType.None,
                MultisampleQuality = 0,
                PresentationInterval = PresentInterval.Immediate,
                PresentFlags = PresentFlags.None,
                SwapEffect = SwapEffect.Discard,
                Windowed = true
            };

            mDevice = new Device(mDriver, 0, DeviceType.Hardware, mControl.Handle, CreateFlags.HardwareVertexProcessing, mParams);
        }

        public void onFrame()
        {
            mDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, System.Drawing.Color.Chartreuse.ToArgb(), 1.0f, 0);
            mDevice.BeginScene();

            mDevice.EndScene();
            try
            {
                mDevice.Present();
            }
            catch(Direct3D9Exception e)
            {
                if (e.ResultCode != ResultCode.DeviceLost)
                    throw;

                if(DeviceLost != null)
                    DeviceLost();

                mDevice.Reset(mParams);

                if (DeviceReset != null)
                    DeviceReset();
            }
        }
    }
}
