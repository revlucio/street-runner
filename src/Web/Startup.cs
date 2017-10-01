using System;
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
            app.UseDeveloperExceptionPage();

            var osm = File.ReadAllText("/Users/luke/code/street-runner/src/Web/map-files/east-london.osm");
            var gpx = File.ReadAllText("/Users/luke/code/street-runner/src/Web/east-london-run.gpx");

            app.Map("/favicon.ico", favicon => {
                favicon.Run(async (context) => {
                    context.Response.StatusCode = 200;
                    await Task.CompletedTask;
                });
            });

            app.MapTo("/street", new StreetsEndpoint(osm).Get);
            app.MapTo("/stats", new StatsEndpoint(osm).Get);
            app.Map("/map", map => 
            {
                map.Run(async (context) => 
                {
                    string response;
                    if (context.Request.Path == string.Empty) 
                    {
                        context.Response.ContentType = "text/plain";
                        response = new MapEndpoint().Get();
                    }
                    else
                    {
                        var mapDir = Path.Combine(AppContext.BaseDirectory, "map-files");
                        var mapFilename = context.Request.Path;
                        osm = File.ReadAllText($"{mapDir}/{mapFilename}.osm");

                        context.Response.ContentType = "text/html";
                        response = new SvgEndpoint(osm, gpx).Get();
                    }

                    await context.Response.WriteAsync(response);
                });
            });

            app.ReturnNotFound();
        }
    }
}
