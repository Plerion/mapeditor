using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.D3D
{
    public class Texture
    {
        private SlimDX.Direct3D9.Texture mTexture;
        private static SlimDX.Direct3D9.Texture gDefault;

        internal SlimDX.Direct3D9.Texture Handle { get { return mTexture; } }

        public Texture()
        {
            if (gDefault == null)
            {
                RenderManager.Instance.Dispatcher.pushFrame(() =>
                    {
                        gDefault = new SlimDX.Direct3D9.Texture(RenderManager.Instance.Device.Device, 1, 1, 1, SlimDX.Direct3D9.Usage.None, SlimDX.Direct3D9.Format.A8R8G8B8, SlimDX.Direct3D9.Pool.Managed);
                        var rc = gDefault.LockRectangle(0, SlimDX.Direct3D9.LockFlags.None);
                        rc.Data.Write((uint)0xFF00FF00);
                        gDefault.UnlockRectangle(0);
                    }
                , false);
            }

            mTexture = gDefault;
        }

        ~Texture()
        {
            if (mTexture != gDefault)
                mTexture.Dispose();
        }
    }
}
