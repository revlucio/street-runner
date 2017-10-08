using System;
using System.IO;

namespace StreetRunner.Web
{
    public static class Settings
    {
        public static string MapFilesDirectory()
        {
            return Path.Combine(AppContext.BaseDirectory, "map-files");
        }
    }
}