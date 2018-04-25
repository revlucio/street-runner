using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace StreetRunner.Web
{
    public class HttpHandler
    {
        public static Action<IApplicationBuilder> Return200Ok()
        {
            return app => {
                app.Run(async (context) => {
                    context.Response.StatusCode = 200;
                    var response = "200 - OK";
                    await context.Response.WriteAsync(response);
                });
            };
        }

        public static string Ok()
        {
            return "200 - OK";
        }
    }
}