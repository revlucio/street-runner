using System.Collections.Generic;

namespace StreetRunner.Web.Repositories
{
    class InMemoryCacheHttpClient : IHttpClient
    {
        private readonly IHttpClient _underlyingHttpClient;
        private static Dictionary<string, string> _cache = new Dictionary<string, string>();

        public InMemoryCacheHttpClient(IHttpClient underlyingHttpClient)
        {
            _underlyingHttpClient = underlyingHttpClient;
        }
        
        public string Get(string url)
        {
            if (_cache.ContainsKey(url) == false)
            {
                var result = _underlyingHttpClient.Get(url);
                _cache[url] = result;
            }

            return _cache[url];
        }
    }
}