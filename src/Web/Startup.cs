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
using StreetRunner.Web.Repositories;

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

            app.Map("/favicon.ico", HttpHandler.Return200Ok());
            app.MapWhen(context => context.Request.Path.Value == "/", HttpHandler.Return200Ok());
            
            app.Map("/api", api =>
            {
                var mapFinder = new FileSystemMapFinder();
                var stravaRunRepository = new StravaRunRepository(new StravaApiClient(), new FileCacheHttpClient(new StravaApiClient()));
                
                api.MapTo("/stats", new StatsEndpoint(mapFinder).Get);
                
                api.Map("/map", mapApi =>
                {
                    mapApi.MapWhen(context => context.Request.Path.HasValue == false, emptyMap =>
                    {
                        emptyMap.MapToJson("", new MapEndpoint(mapFinder).GetJson);    
                    });

                    mapApi.UseRouter(routes =>
                    {
                        routes.MapGet("{mapFilename}", (request, response, routeData) =>
                        {
                            var mapFilename = routeData.Values["mapFilename"].ToString();

                            var svg = new SvgEndpoint(
                                mapFilename, 
                                new FileSystemMapRepository(mapFinder, new FileSystemRunRepository(mapFinder))).Get();
                            
                            response.ContentType = "text/html";
                            return response.WriteAsync(svg);
                        });
                        
                        routes.MapGet("{mapFilename}/street", (request, response, routeData) =>
                        {
                            var mapFilename = routeData.Values["mapFilename"].ToString();
                                
                            var osm = mapFinder.FindMap(mapFilename);
                            var gpx = mapFinder.FindRun("east-london-run");
                    
                            return response.WriteAsync(new StreetsEndpoint(osm, gpx).Get());
                        });
                        
                        routes.MapGet("{mapFilename}/strava", (request, response, routeData) =>
                        {
                            var mapFilename = routeData.Values["mapFilename"].ToString();

                            var svg = new SvgEndpoint(
                                mapFilename, 
                                new FileSystemMapRepository(mapFinder, stravaRunRepository)).Get();
                            
                            response.ContentType = "text/html";
                            return response.WriteAsync(svg);
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
