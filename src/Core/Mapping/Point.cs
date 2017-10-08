using System;

namespace StreetRunner.Core.Mapping
{
    public class Point
    {
        public Point(decimal lat, decimal lon) 
        {
            Lat = lat;
            Lon = lon;
        }

        public decimal Lat { get; }
        public decimal Lon { get; }

        public int CalculateDistanceInMetres(Point otherPoint)
        {
            // disregarding far away points before doing the algorithm
            if (Math.Abs(this.Lon - otherPoint.Lon) > 0.0015m || Math.Abs(this.Lat - otherPoint.Lat) > 0.0015m) 
            {
                return 120;
            }

            var distanceInKiloMetres = DistanceAlgorithm.DistanceBetweenPlaces(
                Convert.ToDouble(this.Lon), 
                Convert.ToDouble(this.Lat), 
                Convert.ToDouble(otherPoint.Lon), 
                Convert.ToDouble(otherPoint.Lat));

            return Convert.ToInt32(distanceInKiloMetres * 1000);
        }
    }
}