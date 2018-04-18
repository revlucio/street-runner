using System;
using System.IO;
using System.Net;

namespace StreetRunner.Web.Repositories
{
    public class FileCacheHttpClient : IHttpClient
    {
        private readonly IHttpClient _underlyingHttpClient;

        public FileCacheHttpClient(IHttpClient underlyingHttpClient)
        {
            _underlyingHttpClient = underlyingHttpClient;
        }
        
        public string Get(string url)
        {
            var escapedUrl = url.Replace('/', '-');
            
            var cacheFile = Path.Combine(Directory.GetCurrentDirectory(), "http-cache", escapedUrl + ".json");
            if (File.Exists(cacheFile) == false)
            {
                var response = _underlyingHttpClient.Get(url);
                File.WriteAllText(cacheFile, response);
            }
            
            return File.ReadAllText(cacheFile);
        }
    }
}