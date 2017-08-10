﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // loggerFactory.AddConsole();
            app.UseDeveloperExceptionPage();

            var osm = File.ReadAllText("/Users/luke/code/street-runner/src/Web/stripped-down-east-london.osm");
            var gpx = File.ReadAllText("/Users/luke/code/street-runner/src/Web/east-london-run.gpx");

            app.Map("/street", street => {
                street.Run(async (context) => {
                    var response = new StreetsEndpoint(osm).Get();

                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync(response);
                });
            });

            app.Run(async (context) =>
            {
                
                var response = new SvgEndpoint(osm, gpx).Get();

                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(response);
            });
        }
    }
}
