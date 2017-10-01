using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StreetRunner;

namespace Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // loggerFactory.AddConsole();
            app.UseDeveloperExceptionPage();

            var osm = File.ReadAllText("/Users/luke/code/street-runner/src/Web/east-london.osm");
            var gpx = File.ReadAllText("/Users/luke/code/street-runner/src/Web/east-london-run.gpx");

            app.Map("/favicon.ico", favicon => {
                favicon.Run(async (context) => {
                    context.Response.StatusCode = 200;
                    await Task.CompletedTask;
                });
            });

            app.MapTo("/street", new StreetsEndpoint(osm).Get);
            app.MapTo("/stats", new StatsEndpoint(osm).Get);
            app.MapTo("/svg", new SvgEndpoint(osm).Get, "text/html");

            app.Run(async (context) =>
            {
                var response = "404 - not found";
                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(response);
            });
        }
    }
}
