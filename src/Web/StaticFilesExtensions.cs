using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;

namespace StreetRunner.Web
{
    public static class StaticFilesExtensions
    {
        public static void UseStaticFiles(this IApplicationBuilder app, string fileDir, string defaultFile)
        {
            var options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add(defaultFile);
            app.UseDefaultFiles(options);

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), fileDir)),
            });
        }
    }
}