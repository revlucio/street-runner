using System;
using System.IO;

namespace PerformanceTests
{
    public static class Settings
    {
        public static string FilesDirectory()
        {
            return Path.Combine(AppContext.BaseDirectory, "files");
        }
    }
}