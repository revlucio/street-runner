using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace StreetRunner.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var port = Environment.GetEnvironmentVariable("PORT");

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>();

            if (string.IsNullOrWhiteSpace(port) == false)
            {
                host = host.UseUrls($"http://+:{port}");
            }

            host.Build().Run();
        }
    }
}
