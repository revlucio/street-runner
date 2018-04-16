using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StreetRunner.Web.Endpoints;

namespace StreetRunner.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDeveloperExceptionPage();

            var dir = Settings.MapFilesDirectory();

            app.Map("/favicon.ico", HttpHandler.Return200Ok());

            app.Map("/api", api =>
            {
                api.MapTo("/stats", new StatsEndpoint(new FileSystemMapFinder()).Get);
                
                api.Map("/map", mapApi =>
                {
                    mapApi.MapWhen(context => context.Request.Path.HasValue == false, emptyMap =>
                    {
                        emptyMap.MapToJson("", new MapEndpoint().GetJson);    
                    });

                    mapApi.UseRouter(routes =>
                    {
                        routes.MapGet("{mapFilename}", (request, response, routeData) =>
                        {
                            var mapFilename = routeData.Values["mapFilename"].ToString();
                                
                            var osm = File.ReadAllText($"{dir}/{mapFilename}.osm");

                            var gpxList = Directory
                                .EnumerateFiles(dir, "*.gpx")
                                .Select(File.ReadAllText);
                    
                            var svg = new SvgEndpoint(osm, gpxList).Get();
                            return response.WriteAsync(svg);
                        });
                        
                        routes.MapGet("{mapFilename}/street", (request, response, routeData) =>
                        {
                            var mapFilename = routeData.Values["mapFilename"].ToString();
                                
                            var osm = File.ReadAllText($"{dir}/{mapFilename}.osm");
                            var gpx = File.ReadAllText($"{dir}/east-london-run.gpx");
                    
                            return response.WriteAsync(new StreetsEndpoint(osm, gpx).Get());
                        });
                        
                        routes.MapGet("{mapFilename}/strava", (request, response, routeData) =>
                        {
                            var mapFilename = routeData.Values["mapFilename"].ToString();
                            var osm = File.ReadAllText($"{dir}/{mapFilename}.osm");
                    
                            return response.WriteAsync(new StravaEndpoint(new RestHttpClient(), osm).Get());
                        });
                    });
                });

                api.MapToJson("", new ApiRootEndpoint().Get);
                api.ReturnNotFound();
            });

            app.ReturnNotFound();
        }
    }
}
