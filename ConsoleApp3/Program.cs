using System;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace BaseCreator
{


    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        static void Main(string[] args)
        {

            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_SHOW);
            var reader = new DataReader();
            using (ZipArchive archive = ZipFile.OpenRead(reader.GetArchive()))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.Contains("_UO"))
                    {
                        reader.UnpackDataFromArchive(entry);
                    }
                }
            }
            GC.Collect();
        }
    }
}
