using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StreetRunner.Core.Mapping;
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

            app.UseStaticFiles("wwwroot", "public/index.html");

            app.ViewDirectoryAndFiles("/cache", FileCacheHttpClient.CacheDirectory);
            app.ViewDirectoryAndFiles("/logs", LoggerHttpClient.LogDirectory);

            app.Map("/favicon.ico", HttpHandler.Return200Ok());
            app.MapRootTo(HttpHandler.Ok());
            
            app.Map("/api", api =>
            {
                var mapFinder = new FileSystemMapFinder();
                var cachedCoveredStreetCalculator = new CacheCoveredStreetCalculator(new CoveredStreetCalculator());
             
                api.UseMiddleware<AuthenticateWithStrava>();
                
                api.MapGetToJson("", new ApiRootEndpoint().Get);
                api.MapTo("/stats", new StatsEndpoint(mapFinder).Get);
                
                api.Map("/map", mapApi =>
                {
                    mapApi.MapGetToJson("", new MapEndpoint(mapFinder).GetJson);

                    mapApi.MapGetToHtml("{mapFilename}", (request, response, routeData) =>
                    {
                        var svgEndpoint = new SvgEndpoint(
                            routeData.Values["mapFilename"].ToString(),
                            new FileSystemMapRepository(
                                mapFinder, new FileSystemRunRepository(mapFinder), cachedCoveredStreetCalculator));
                        
                        return svgEndpoint.Get();
                    });

                    mapApi.UseRouter(routes =>
                    {
                        routes.MapGet("{mapFilename}/street", (request, response, routeData) =>
                        {
                            var mapFilename = routeData.Values["mapFilename"].ToString();
                                
                            var osm = mapFinder.FindMap(mapFilename);
                            var gpx = mapFinder.FindRun("east-london-run");
                    
                            return response.WriteAsync(new StreetsEndpoint(osm, gpx).Get());
                        });
                        
                        routes.MapGet("{mapFilename}/strava", (request, response, routeData) =>
                        {
                            var token = request.Cookies["strava-token"];
                            var mapFilename = routeData.Values["mapFilename"].ToString();

                            var httpClient = new LoggerHttpClient(new ApiClient());
                            var runRepository = new StravaRunRepository(
                                httpClient, 
                                new FileCacheHttpClient(httpClient), 
                                token);
                            var stravaRunRepository = runRepository;
                            var svg = new SvgEndpoint(
                                mapFilename, 
                                new FileSystemMapRepository(mapFinder, stravaRunRepository, cachedCoveredStreetCalculator))
                                .Get();
                            
                            response.ContentType = "text/html";
                            return response.WriteAsync(svg);
                        });
                    });
                });
                
                api.ReturnNotFound();
            });

            app.ReturnNotFound();
        }
    }
}
