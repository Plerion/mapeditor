using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MapEditor.IO
{
    internal class MpqManager
    {
        [DllImport("Stormlib.dll")]
        private static extern bool SFileOpenArchive(string szMpqName, uint dwPriority, StreamFlags dwFlags, out IntPtr phMPQ);

        [DllImport("Stormlib.dll")]
        private static extern bool SFileOpenFileEx(IntPtr hMpq, string szFileName, uint dwSearchScope, out IntPtr phFile);

        [DllImport("Stormlib.dll")]
        private static extern uint SFileGetFileSize(IntPtr hFile, out uint pdwFileSizeHigh);

        [DllImport("Stormlib.dll")]
        private static extern uint SFileSetFilePointer(IntPtr hFile, uint lFilePos, ref uint plFilePosHigh, uint dwMoveMethod);

        [DllImport("Stormlib.dll")]
        private static extern bool SFileReadFile(IntPtr hFile, byte[] lpBuffer, int dwToRead, out int pdwRead, IntPtr lpOverlapped);

        [DllImport("Stormlib.dll")]
        private static extern bool SFileCloseFile(IntPtr hFile);

        [DllImport("Stormlib.dll")]
        private static extern bool SFileHasFile(IntPtr hMpq, string szFileName);

        enum StreamFlags : uint
        {
            STREAM_FLAG_READ_ONLY       = 0x100,
            STREAM_FLAG_WRITE_SHARE     = 0x200,
            MPQ_OPEN_NO_LISTFILE        = 0x10000,
        }

        private string mBasePath;

        public MpqManager(string basePath)
        {
            mBasePath = basePath;
        }

        public bool LoadArchives()
        {
            var attribs = System.IO.File.GetAttributes(mBasePath);
            if ((attribs & System.IO.FileAttributes.Directory) == 0)
                return false;

            if (mBasePath.EndsWith('\\') == false)
                mBasePath += '\\';

            var dataPath = mBasePath + "data";

            return true;
        }
    }
}
