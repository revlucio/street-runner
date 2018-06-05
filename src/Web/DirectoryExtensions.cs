using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace StreetRunner.Web
{
    public static class DirectoryExtensions
    {
        public static void ViewDirectoryAndFiles(this IApplicationBuilder app, string url, string directory)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(directory),
                RequestPath = new PathString(url),
            });
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(directory),
                RequestPath = new PathString(url),
            });
        }
    }
}