using System.Collections.Generic;
using System.Linq;
using Shouldly;
using StreetRunner.Core.Mapping;
using Xunit;

namespace StreetRunner.UnitTests.Domain
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

            map.Streets.Single().Covered.ShouldBeFalse();
        }

        [Fact]
        public void RunDoesNotCoverStreetAtAll_IsNotCovered()
        {
            var isCovered = IsCovered(
                new List<Point> 
                {
                    new Point(0m, 0m),
                    new Point(100m, 100m),
                }, 
                new List<Point> {
                    new Point(200m, 200m),
                    new Point(300m, 300m),
                });
            isCovered.ShouldBeFalse();
        }

        [Fact]
        public void RunCoversStreetPerfectly_IsCovered()
        {
            var isCovered = IsCovered(
                new List<Point> 
                {
                    new Point(0m, 0m),
                    new Point(100m, 100m),
                }, 
                new List<Point> {
                    new Point(0m, 0m),
                    new Point(100m, 100m),
                });
            isCovered.ShouldBeTrue();
        }

        [Fact]
        public void RunIsAtLeast1MetresToAnyStreetPoint_IsCovered() 
        {
            // 0.00009 is approx 1m
            var ninetyMetres = 0.000008m;
            var isCovered = IsCovered(
                new List<Point> 
                {
                    new Point(50m, 50m),
                }, 
                new List<Point> {
                    new Point(50m + ninetyMetres, 50m),
                });
            isCovered.ShouldBeTrue();
        }

        private bool IsCovered(IEnumerable<Point> streetPoints, IEnumerable<Point> runPoints) {
            var street = new Street("name", streetPoints);
            var run = new Run(runPoints, "time");

            var map = new Map(new List<Street>{street}, new List<IRun>{run});

            return map.Streets.Single().Covered;
        }
    }
}
