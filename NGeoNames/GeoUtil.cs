using NGeoNames.Entities;
using System;

namespace NGeoNames
{
    internal static class GeoUtil
    {
        public static double Deg2Rad(double deg)
        {
            return (Math.PI / 180.0) * deg;
        }

        public static double DistanceTo(GeoName src, GeoName dest)
        {
            double dLat = GeoUtil.Deg2Rad(dest.Latitude - src.Latitude);
            double dLon = GeoUtil.Deg2Rad(dest.Longitude - src.Longitude);
            return 6371 * (2 * Math.Asin(Math.Min(1, Math.Sqrt(Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(GeoUtil.Deg2Rad(src.Latitude)) * Math.Cos(GeoUtil.Deg2Rad(dest.Latitude)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2)))));
        }
    }
}
