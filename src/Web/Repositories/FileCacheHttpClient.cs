using System.IO;

namespace StreetRunner.Web.Repositories
{
    public class FileCacheHttpClient : IHttpClient
    {
        private const string CacheDirectory = "/data/street-runner/http-cache";

        private readonly IHttpClient _underlyingHttpClient;

        public FileCacheHttpClient(IHttpClient underlyingHttpClient)
        {
            _underlyingHttpClient = underlyingHttpClient;
        }
        
        public string Get(string url)
        {
            var escapedUrl = url.Replace('/', '-');
            
            var cacheFile = Path.Combine(CacheDirectory, escapedUrl + ".json");
            if (File.Exists(cacheFile) == false)
            {
                var response = _underlyingHttpClient.Get(url);
                
                if (Directory.Exists(CacheDirectory) == false)
                {
                    Directory.CreateDirectory(CacheDirectory);
                }
                File.WriteAllText(cacheFile, response);
            }
            
            return File.ReadAllText(cacheFile);
        }
    }
}