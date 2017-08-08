using System;

namespace StreetRunner
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

        public int CalculateDistanceInMetres(Point point)
        {
            var distanceInKiloMetres = DistanceAlgorithm.DistanceBetweenPlaces(
                Convert.ToDouble(this.Lon), 
                Convert.ToDouble(this.Lat), 
                Convert.ToDouble(point.Lon), 
                Convert.ToDouble(point.Lat));

            return Convert.ToInt32(distanceInKiloMetres * 1000);
        }
    }
}