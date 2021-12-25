using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickLauncher.Utils
{
    class FileUtils
    {
        public static void Open(string path)
        {
            Process.Start(path);
        }

        public static void Open(string path, string program)
        {
            Process.Start(program, path);
        }
    }
}
