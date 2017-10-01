using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Web
{
    public static class ApplicationBuilderExtensions 
    {
        public static IApplicationBuilder MapTo(
            this IApplicationBuilder application, 
            string url, 
            Func<string> func, 
            string contentType = "text/plain") 
        {
            return application.Map(url, api => 
            {
                api.Run(async (context) => 
                {
                    var response = func();
                    context.Response.ContentType = contentType;
                    await context.Response.WriteAsync(response);
                });
            });
        }    
    }
}
