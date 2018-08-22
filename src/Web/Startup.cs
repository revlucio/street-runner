using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StreetRunner.Core.Mapping;
using StreetRunner.Web.Endpoints;
using StreetRunner.Web.Repositories;
using IHttpClient = StreetRunner.Web.Endpoints.IHttpClient;

namespace StreetRunner.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();

            services.AddScoped<StreetRunner.Web.Repositories.IHttpClient>(x => new LoggerHttpClient(new ApiClient()));
            services.AddScoped<IMapFinder, FileSystemMapFinder>();
            services.AddScoped<ICoveredStreetCalculator>(x => new CacheCoveredStreetCalculator(new CoveredStreetCalculator()));
            services.AddScoped<IRunRepository, FileSystemRunRepository>();
            services.AddScoped<IMapRepository, FileSystemMapRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles("wwwroot", "public/index.html");

            app.ViewDirectoryAndFiles("/cache", FileCacheHttpClient.CacheDirectory);
            app.ViewDirectoryAndFiles("/logs", LoggerHttpClient.LogDirectory);

            app.Map("/favicon.ico", HttpHandler.Return200Ok());
            app.MapRootTo(HttpHandler.Ok());
            
            var httpClient = app.ApplicationServices.GetService<StreetRunner.Web.Repositories.IHttpClient>();
            
            app.Map("/api", api =>
            {
                var mapFinder = api.ApplicationServices.GetService<IMapFinder>();
                var coveredStreetCalculator = api.ApplicationServices.GetService<ICoveredStreetCalculator>();
                var mapRepository = api.ApplicationServices.GetService<IMapRepository>();
             
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
                            mapRepository);
                        
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
                            // the map should already be loaded
                            var mapFilename = routeData.Values["mapFilename"].ToString();

                            // the run repo should already be loaded
                            var stravaRunRepository = new StravaRunRepository(
                                httpClient, 
                                new FileCacheHttpClient(httpClient), 
                                request.Cookies["strava-token"]);
                            
                            var svg = new SvgEndpoint(
                                mapFilename, 
                                new FileSystemMapRepository(mapFinder, stravaRunRepository, coveredStreetCalculator))
                                .Get();
                            
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
