namespace StreetRunner.Web.Endpoints
{
    public class ApiRootEndpoint
    {
        public string Get()
        {
            return @"{
""url"": ""http://localhost:5000/map""
}";
        }
    }
}