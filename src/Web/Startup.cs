using System;
using System.IO;
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

            app.MapTo("/stats", () =>
            {
                var osm = File.ReadAllText($"{dir}/east-london.osm");
                return new StatsEndpoint(osm).Get();
            });

            app.Map("/api", api =>
            {
                api.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(@"{
""url"": ""http://localhost:5000/map""
}");
                });
            });
            
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
                        
                        var osm = File.ReadAllText($"{Settings.MapFilesDirectory()}/{mapFilename}.osm");
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
                            response = new SvgEndpoint(osm, gpx).Get();
                        }
                    }

                    await context.Response.WriteAsync(response);
                });
            });

            app.ReturnNotFound();
        }
    }
}
