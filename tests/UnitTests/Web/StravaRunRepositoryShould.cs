using System.Linq;
using Microsoft.AspNetCore.Http;
using Shouldly;
using StreetRunner.Web.Repositories;
using Xunit;

namespace StreetRunner.UnitTests.Web
{
    public class StravaRunRepositoryShould
    {
        [Fact]
        public void ReturnEmptyListWhenNoIdsReturned()
        {
            var httpAccessor = new HttpContextAccessor {HttpContext = new DefaultHttpContext()};
            var stubApiClient = new StubHttpClient("[]");
            var repo = new StravaRunRepository(stubApiClient, new StubHttpClient(string.Empty), httpAccessor);

            var runs = repo.GetAll();
            
            runs.ShouldBeEmpty();
        }
        
        [Fact]
        public void ReturnSingleItem()
        {
            var httpAccessor = new HttpContextAccessor {HttpContext = new DefaultHttpContext()};
            var stubApiClient = new StubHttpClient(@"[{""id"": 5}]");
            var runJson = @"
[
    {
        ""data"": [
            [ 3, 3 ],
            [ 33.3, 44.4 ]
        ]
    }
]";
            var runHttpClient = new StubHttpClient(runJson);
            var repo = new StravaRunRepository(stubApiClient, runHttpClient, httpAccessor);

            var runs = repo.GetAll();
            runs.Count().ShouldBe(1);
        }
        
        [Fact]
        public void ReturnEmptyWhenRunsAreInvalid()
        {
            var httpAccessor = new HttpContextAccessor {HttpContext = new DefaultHttpContext()};
            var stubApiClient = new StubHttpClient(@"[{""id"": 5}]");
            var runJson = @"[{}]";
            var runHttpClient = new StubHttpClient(runJson);
            var repo = new StravaRunRepository(stubApiClient, runHttpClient, httpAccessor);

            var runs = repo.GetAll();
            
            runs.ShouldBeEmpty();
        }
    }
}