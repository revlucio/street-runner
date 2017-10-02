using System.Collections.Generic;
using System.Linq;
using StreetRunner.Mapping;
using Xunit;

namespace tests
{
    public class MapShouldCoverStreets
    {
        [Fact]
        public void NoRuns_IsNotCovered() 
        {
            var street = new Street("name", new List<Point> 
            {
                { new Point(0m, 0m) },
                { new Point(100m, 100m) },
            });
            var map = new Map(new List<Street>{street});

            Assert.False(map.Streets.Single().Covered);
        }

        [Fact]
        public void RunDoesNotCoverStreetAtAll_IsNotCovered() 
        {
            Assert.False(IsCovered(
                new List<Point> 
                {
                    new Point(0m, 0m),
                    new Point(100m, 100m),
                }, 
                new List<Point> {
                    new Point(200m, 200m),
                    new Point(300m, 300m),
                })
            );
        }

        [Fact]
        public void RunCoversStreetPerfectly_IsCovered() 
        {
            Assert.True(IsCovered(
                new List<Point> 
                {
                    new Point(0m, 0m),
                    new Point(100m, 100m),
                }, 
                new List<Point> {
                    new Point(0m, 0m),
                    new Point(100m, 100m),
                })
            );
        }

        [Fact]
        public void RunIsAtLeast100MetresToAnyStreetPoint_IsCovered() 
        {
            // 0.0009 is approx 100m
            var ninetyMetres = 0.0008m;
            Assert.True(IsCovered(
                new List<Point> 
                {
                    new Point(50m, 50m),
                }, 
                new List<Point> {
                    new Point(50m + ninetyMetres, 50m),
                })
            );
        }

        private bool IsCovered(IEnumerable<Point> streetPoints, IEnumerable<Point> runPoints) {
            var street = new Street("name", streetPoints);
            var map = new Map(new List<Street>{street});
            var run = new Run("name", runPoints);
            map.AddRun(run);

            return map.Streets.Single().Covered;
        }
    }
}
