using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MapEditor.D3D
{
    public class RenderDispatcher
    {
        private class Frame
        {
            public Action Callback { get; set; }
            public AutoResetEvent CompletionEvent { get; set; }
            public bool IsAsync{get;set;}
        }

        private List<Frame> mFrames = new List<Frame>();
        public Thread MainThread { get; internal set; }

        internal void runFrame()
        {
            lock (mFrames)
            {
                for (int i = 0; i < 25 && mFrames.Count > 0; ++i)
                {
                    Frame f = mFrames.First();
                    mFrames.RemoveAt(0);
                    f.Callback();
                    if (f.IsAsync == false)
                        f.CompletionEvent.Set();
                }
            }
        }

        public void pushFrame(Action action, bool async = true)
        {
            if (async == false)
            {
                if (System.Threading.Thread.CurrentThread.ManagedThreadId == MainThread.ManagedThreadId)
                {
                    action();
                    return;
                }
            }

            Frame frame = new Frame()
            {
                Callback = action,
                IsAsync = async
            };

            if(async == false)
                frame.CompletionEvent = new AutoResetEvent(false);

            lock(mFrames)
                mFrames.Add(frame);

            frame.CompletionEvent.WaitOne();
        }
    }
}
