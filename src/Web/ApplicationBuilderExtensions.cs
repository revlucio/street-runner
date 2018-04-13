using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

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
            Func<string> func)
        {
            return app.MapTo(url, func, "application/json");
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
    }
}
