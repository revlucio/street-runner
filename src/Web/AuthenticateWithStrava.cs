using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;

namespace StreetRunner.Web
{
    public class AuthenticateWithStrava
    {
        private readonly RequestDelegate _next;

        public AuthenticateWithStrava(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;
            
            var applicationId = "20675";
            if (Environment.GetEnvironmentVariables().Contains("STRAVA_SECRET") == false)
            {
                throw new ArgumentException("The STRAVA_SECRET environment variable must be set");
            }
            var secret = Environment.GetEnvironmentVariable("STRAVA_SECRET");

            var url = request.GetUrlWithoutQueryString();
            
            if (request.Query.ContainsKey("code") == false && request.Cookies.ContainsKey("strava-token") == false)
            {
                var authUrl = QueryHelpers.AddQueryString(
                    $"{Settings.StravaUrl}/oauth/authorize",
                    new Dictionary<string, string>
                    {
                        {"client_id", applicationId},
                        {"redirect_uri", url},
                        {"response_type", "code"},
                        {"scope", "view_private"},
                    });
                response.Redirect(authUrl);
                return Task.CompletedTask;
            }

            if (request.Query.ContainsKey("code") && request.Cookies.ContainsKey("strava-token") == false)
            {
                var code = request.Query["code"].Single();

                var client = new HttpClient();
                var authUrl = QueryHelpers.AddQueryString(
                    $"{Settings.StravaUrl}/oauth/token",
                    new Dictionary<string, string>
                    {
                        {"client_id", applicationId},
                        {"client_secret", secret},
                        {"code", code},
                    });
                
                var tokenResponseMessage = client.PostAsync(authUrl, null).Result;
                tokenResponseMessage.EnsureSuccessStatusCode();

                var content = tokenResponseMessage.Content.ReadAsStringAsync().Result;
                var token = JObject.Parse(content).Value<string>("access_token");
                response.Cookies.Append("strava-token", token);
                Console.WriteLine(url);
                response.Redirect(url);
                return Task.CompletedTask;
            }
            
            return _next(context);
        }
    }
}