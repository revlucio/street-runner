using Microsoft.AspNetCore.Http;

namespace StreetRunner.Web
{
    public static class HttpRequestExtensions
    {
        public static string GetBaseUrl(this HttpRequest request)
        {
            return $"{request.Scheme}://{request.Host}";
        }
        
        public static string GetUrlWithoutQueryString(this HttpRequest request)
        {
            return $"{request.GetBaseUrl()}{request.PathBase}{request.Path}";
        }
    }
}