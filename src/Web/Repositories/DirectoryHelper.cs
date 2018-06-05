using System.IO;

namespace StreetRunner.Web.Repositories
{
    public static class DirectoryHelper
    {
        public static string EnsureExists(string directory)
        {
            if (Directory.Exists(directory) == false)
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }
    }
}