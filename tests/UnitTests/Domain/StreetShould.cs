using StreetRunner.Core.Mapping;
using Xunit;

namespace StreetRunner.UnitTests.Domain
{
    public class StreetShould
    {
        [Fact]
        public void GenerateNameFromPointsWhenNotGiven()
        {
            var street = new Street(string.Empty, new[] {new Point(1, 1)});
            
            Assert.Equal("11", street.Name);
        }
    }
}