using System;
using StreetRunner.Web.Repositories;

namespace StreetRunner.UnitTests.Web
{
    public class StubHttpClient : IHttpClient
    {
        private readonly string _stubReponse;

        public StubHttpClient(string stubReponse)
        {
            _stubReponse = stubReponse;
        }

        public string Get(string url)
        {
            return _stubReponse;
        }
    }
}