using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StreetRunner.Web.Endpoints;

namespace StreetRunner.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDeveloperExceptionPage();

            var dir = Settings.MapFilesDirectory();

            app.Map("/favicon.ico", HttpHandler.Return200Ok());

            app.MapTo("/api/stats", new StatsEndpoint(new FileSystemMapFinder()).Get);
            app.MapToJson("/api", new ApiRootEndpoint().Get);
            
            app.Map("/map", map => 
            {
                map.Run(async (context) => 
                {
                    context.Response.ContentType = "text/plain";
                    string response;

                    if (context.Request.Path == string.Empty) 
                    {
                        response = new MapEndpoint().Get();
                    }
                    else
                    {
                        var mapFilename = context.Request.Path
                            .ToString()
                            .Replace("/street", string.Empty)
                            .Replace("/strava", string.Empty);
                        
                        var osm = File.ReadAllText($"{dir}/{mapFilename}.osm");
                        var gpxList = Directory.EnumerateFiles(dir, "*.gpx")
                            .Select(File.ReadAllText);
                        var gpx = File.ReadAllText($"{dir}/east-london-run.gpx");

                        Console.WriteLine(context.Request.Path.ToString());

                        if (context.Request.Path.ToString().EndsWith("/street"))
                        {
                            response = new StreetsEndpoint(osm, gpx).Get();
                        }
                        else if (context.Request.Path.ToString().EndsWith("/strava"))
                        {
                            response = new StravaEndpoint(new RestHttpClient(), osm).Get();
                        }
                        else
                        {
                            context.Response.ContentType = "text/html";
                            response = new SvgEndpoint(osm, gpxList).Get();
                        }
                    }

                    await context.Response.WriteAsync(response);
                });
            });

            app.ReturnNotFound();
        }
    }
}
