using System.Threading.Tasks;
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
            services
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddRouting()
                .AddScoped<IMapFinder, FileSystemMapFinder>()
                .AddScoped<ICoveredStreetCalculator>(_ => new CacheCoveredStreetCalculator(new CoveredStreetCalculator()))
                .AddScoped<IRunRepository, FileSystemRunRepository>()
                .AddScoped<IMapRepository, FileSystemMapRepository>()
                .AddScoped<Repositories.IHttpClient, ApiClient>()
                .AddScoped<MapEndpoint>()
                .AddScoped<ApiRootEndpoint>()
                .AddScoped<StatsEndpoint>()
                .AddScoped<StravaRunRepository>()
                ;
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
            
            var stravaRunRepository = new StravaRunRepository(
                httpClient, 
                new FileCacheHttpClient(httpClient), 
                services.GetService<IHttpContextAccessor>());

            var mapRepo = new FileSystemMapRepository(mapFinder, stravaRunRepository, coveredStreetCalculator);

            app.Map("/fake-strava", fakeStrava =>
            {
                fakeStrava.Map("/oauth", fakeAuth =>
                {
                    fakeAuth.Run(async context =>
                    {
                        var url = context.Request.Query["redirect_uri"];
                        context.Response.Redirect(url);
                        context.Response.Cookies.Append("strava-token", "fake-token");
                        await Task.CompletedTask;
                    });    
                });
                
                fakeStrava.ReturnNotFound();
            });
            
            app.Map("/api", api =>
            {
                api.UseMiddleware<AuthenticateWithStrava>();
                
                api.MapGetToJson("", services.GetService<ApiRootEndpoint>().Get);
                api.MapTo("/stats", services.GetService<StatsEndpoint>().Get);
                
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
                            var json = new StreetsEndpoint(mapRepo.Get(mapFilename)).Get();
                            response.ContentType = "application/json";
                            return response.WriteAsync(json);
                        });

                        routes.MapGet("{mapFilename}/strava", (request, response, routeData) =>
                        {
                            var mapFilename = routeData.Values["mapFilename"].ToString();
                            
                            var svg = new SvgEndpoint(mapRepo).Get(mapFilename);
                            
                            response.ContentType = "text/html";
                            return response.WriteAsync(svg);
                        });
                        
                        routes.MapGet("{mapFilename}/summary", (request, response, routeData) =>
                        {
                            var mapFilename = routeData.Values["mapFilename"].ToString();
                            
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