using System.Linq;
using StreetRunner.Web.Repositories;
using Xunit;

namespace StreetRunner.UnitTests.Web
{
    public class StravaRunRepositoryShould
    {
        [Fact]
        public void ReturnEmptyListWhenNoIdsReturned()
        {
            var stubApiClient = new StubHttpClient("[]");
            var repo = new StravaRunRepository(stubApiClient, new StubHttpClient(string.Empty), "token");

            var runs = repo.GetAll();
            Assert.Equal(0, runs.Count());
        }
        
        [Fact]
        public void ReturnSingleItem()
        {
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
            var repo = new StravaRunRepository(stubApiClient, runHttpClient, "token");

            var runs = repo.GetAll();
            Assert.Equal(1, runs.Count());
        }
        
        [Fact]
        public void ReturnEmptyWhenRunsAreInvalid()
        {
            var stubApiClient = new StubHttpClient(@"[{""id"": 5}]");
            var runJson = @"[{}]";
            var runHttpClient = new StubHttpClient(runJson);
            var repo = new StravaRunRepository(stubApiClient, runHttpClient, "token");

            var runs = repo.GetAll();
            Assert.Equal(0, runs.Count());
        }
    }
}