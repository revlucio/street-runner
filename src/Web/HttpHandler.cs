using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace StreetRunner.Web
{
    public class HttpHandler
    {
        public static Action<IApplicationBuilder> Return200Ok()
        {
            return app => {
                app.Run(async (context) => {
                    context.Response.StatusCode = 200;
                    await Task.CompletedTask;
                });
            };
        }
    }
}