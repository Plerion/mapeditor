using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.D3D
{
    internal class TextureFetcher
    {
        private class WorkItem
        {
            public string FileName { get; set; }
            public Texture Texture { get; set; }
        }

        private System.Threading.Thread mLoadThread;
        private volatile bool mIsRunning = true;
        private List<WorkItem> mItems = new List<WorkItem>();

        public int WorkLoad { get { return mItems.Count; } }

        public TextureFetcher()
        {
            mLoadThread = new System.Threading.Thread(loadProc);
            mLoadThread.Start();
        }

        public void pushRequest(string file, Texture tex)
        {
            WorkItem item = new WorkItem()
            {
                FileName = file,
                Texture = tex
            };

            lock (mItems)
                mItems.Add(item);
        }

        private void loadProc()
        {
            while(mIsRunning)
            {

            }
        }


    }
}
