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

            services.AddScoped<IMapFinder, FileSystemMapFinder>();
            services.AddScoped<ICoveredStreetCalculator>(x => new CacheCoveredStreetCalculator(new CoveredStreetCalculator()));
            services.AddScoped<IRunRepository, FileSystemRunRepository>();
            services.AddScoped<IMapRepository, FileSystemMapRepository>();

            services.AddScoped<MapEndpoint>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles("wwwroot", "public/index.html");

            app.ViewDirectoryAndFiles("/cache", FileCacheHttpClient.CacheDirectory);
            app.ViewDirectoryAndFiles("/logs", LoggerHttpClient.LogDirectory);

            app.Map("/favicon.ico", HttpHandler.Return200Ok());
            app.MapRootTo(HttpHandler.Ok());

            var services = app.ApplicationServices;
            var httpClient = services.GetService<Repositories.IHttpClient>();
            var mapFinder = services.GetService<IMapFinder>();
            var coveredStreetCalculator = services.GetService<ICoveredStreetCalculator>();
            
            app.Map("/api", api =>
            {
                api.UseMiddleware<AuthenticateWithStrava>();
                
                api.MapGetToJson("", new ApiRootEndpoint().Get);
                api.MapTo("/stats", new StatsEndpoint(mapFinder).Get);
                
                api.Map("/map", mapApi =>
                {
                    mapApi.MapGetToJson("", services.GetService<MapEndpoint>().GetJson);

                    mapApi.MapGetToHtml("{mapFilename}", (request, response, routeData) =>
                    {
                        var svgEndpoint = services.GetService<SvgEndpoint>();
                        var mapName = routeData.Values["mapFilename"].ToString();
                        return svgEndpoint.Get(mapName);
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
                            // the map should already be loaded
                            var mapFilename = routeData.Values["mapFilename"].ToString();

                            // the run repo should already be loaded
                            var stravaRunRepository = new StravaRunRepository(
                                httpClient, 
                                new FileCacheHttpClient(httpClient), 
                                request.Cookies["strava-token"]);
                            
                            var svg = new SvgEndpoint(new FileSystemMapRepository(mapFinder, stravaRunRepository, coveredStreetCalculator))
                                .Get(mapFilename);
                            
                            response.ContentType = "text/html";
                            return response.WriteAsync(svg);
                        });
                        
                        routes.MapGet("{mapFilename}/summary", (request, response, routeData) =>
                        {
                            var mapFilename = routeData.Values["mapFilename"].ToString();
                            
                            var stravaRunRepository = new StravaRunRepository(
                                httpClient, 
                                new FileCacheHttpClient(httpClient), 
                                request.Cookies["strava-token"]);
                            
                            var mapRepo = new FileSystemMapRepository(mapFinder, stravaRunRepository,
                                coveredStreetCalculator);

                            var json = new SummaryEndpoint(mapRepo.Get(mapFilename)).Get();
                            
                            response.ContentType = "application/json";
                            return response.WriteAsync(json);
                        });
                    });
                });
                
                api.ReturnNotFound();
            });

            app.ReturnNotFound();
        }
    }
}
