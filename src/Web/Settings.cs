using System;

namespace StreetRunner.Web
{
    public static class Settings
    {
        public static string StravaUrl => Environment.GetEnvironmentVariable("STRAVA_URL") ?? "https://www.strava.com";
    }
}