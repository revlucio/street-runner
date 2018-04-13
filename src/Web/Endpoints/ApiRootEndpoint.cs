using Newtonsoft.Json.Linq;

namespace StreetRunner.Web.Endpoints
{
    public class ApiRootEndpoint
    {
        public JObject Get()
        {
            return JObject.FromObject(new
            {
                url = $"{Settings.UrlRoot}/map"
            });
        }
    }
}