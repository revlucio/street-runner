using Newtonsoft.Json.Linq;

namespace StreetRunner.Web.Endpoints
{
    public class ApiRootEndpoint
    {
        public JObject Get()
        {
            return JObject.FromObject(new
            {
                urls = new []
                {
                    $"/api/map",  
                    $"{Settings.UrlRoot}/api/stats",  
                } 
            });
        }
    }
}