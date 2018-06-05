using System.IO;

namespace StreetRunner.Web.Repositories
{
    public class FileCacheHttpClient : IHttpClient
    {
        public static string CacheDirectory => DirectoryHelper.EnsureExists("/data/street-runner/http-cache");

        private readonly IHttpClient _underlyingHttpClient;

        public FileCacheHttpClient(IHttpClient underlyingHttpClient)
        {
            _underlyingHttpClient = underlyingHttpClient;
        }
        
        public string Get(string url)
        {
            var cacheFile = Path.Combine(CacheDirectory, url.EncodeForFileSystem() + ".json");
            if (File.Exists(cacheFile) == false)
            {
                var response = _underlyingHttpClient.Get(url);
                File.WriteAllText(cacheFile, response);
            }
            
            return File.ReadAllText(cacheFile);
        }
    }
}