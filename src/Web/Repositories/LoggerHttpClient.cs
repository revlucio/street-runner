using System;
using System.IO;

namespace StreetRunner.Web.Repositories
{
    public class LoggerHttpClient : IHttpClient
    {
        public static string LogDirectory => DirectoryHelper.EnsureExists("/data/street-runner/http-log");
        
        private readonly IHttpClient _underlyingHttpClient;

        public LoggerHttpClient(IHttpClient underlyingHttpClient)
        {
            _underlyingHttpClient = underlyingHttpClient;
        }
        
        public string Get(string url)
        {
            var logFile = Path.Combine(LogDirectory, DateTime.Now.ToString("O") + url.EncodeForFileSystem() + ".txt");
            var response = _underlyingHttpClient.Get(url);
            File.WriteAllText(logFile, response);
            return response;
        }
    }
}