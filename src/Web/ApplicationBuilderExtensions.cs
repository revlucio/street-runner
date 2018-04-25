using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json.Linq;

namespace StreetRunner.Web
{
    public static class ApplicationBuilderExtensions 
    {
        public static IApplicationBuilder MapTo(
            this IApplicationBuilder app, 
            PathString url, 
            Func<string> func, 
            string contentType = "text/plain") 
        {
            return app.Map(url, api => 
            {
                api.Run(async (context) => 
                {
                    var response = func();
                    context.Response.ContentType = contentType;
                    await context.Response.WriteAsync(response);
                });
            });
        }
        
        public static IApplicationBuilder MapToJson(
            this IApplicationBuilder app, 
            PathString url, 
            Func<JObject> func)
        {
            return app.MapTo(url, () => func().ToString(), "application/json");
        }

        public static void ReturnNotFound(this IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                var response = "404 - not found";
                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(response);
            });
        }

        public static IApplicationBuilder MapGetToJson(
            this IApplicationBuilder app,
            string template,
            Func<HttpRequest, HttpResponse, RouteData, JObject> func)
        {
            return app.UseRouter(routes =>
            {
                routes.MapGet(template, (req, res, routeData) =>
                {
                    var json = func(req, res, routeData);
                    res.ContentType = "application/json";
                    return res.WriteAsync(json.ToString());                    
                });
            });
        }
        
        public static IApplicationBuilder MapGetToJson(
            this IApplicationBuilder app,
            string template,
            Func<JObject> func)
        {
            return app.UseRouter(routes =>
            {
                routes.MapGet(template, (req, res, routeData) =>
                {
                    var json = func();
                    res.ContentType = "application/json";
                    return res.WriteAsync(json.ToString());                    
                });
            });
        }
        
        public static IApplicationBuilder MapGetToHtml(
            this IApplicationBuilder app,
            string template,
            Func<HttpRequest, HttpResponse, RouteData, string> func)
        {
            return app.UseRouter(routes =>
            {
                routes.MapGet(template, (req, res, routeData) =>
                {
                    var json = func(req, res, routeData);
                    res.ContentType = "text/html";
                    return res.WriteAsync(json);                    
                });
            });
        }

        public static IApplicationBuilder MapRootTo(
            this IApplicationBuilder app,
            string response)
        {
            return app.MapWhen(context => context.Request.Path.Value == "/", root =>
            {
                root.Run(context => context.Response.WriteAsync(response));
            });
        }
    }
}
