using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.D3D
{
    public class TextureManager
    {
        private Dictionary<int, WeakReference<Texture>> mCache = new Dictionary<int, WeakReference<Texture>>();
        private List<TextureFetcher> mFetcher = new List<TextureFetcher>();

        public TextureManager()
        {
            for (int i = 0; i < 4; ++i)
                mFetcher.Add(new TextureFetcher());
        }

        public Texture getTexture(string file)
        {
            int hash = file.ToLower().GetHashCode();
            Texture ret;

            lock (mCache)
            {
                if (mCache.ContainsKey(hash))
                {
                    if (mCache[hash].TryGetTarget(out ret) == true)
                        return ret;
                }

                ret = new Texture();
                mCache.Add(hash, new WeakReference<Texture>(ret));

                var fetcher = mFetcher.First(t => t.WorkLoad == mFetcher.Min(f => f.WorkLoad));
                fetcher.pushRequest(file, ret);
            }

            return ret;
        }
    }
}
