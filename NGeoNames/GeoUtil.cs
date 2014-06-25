using NGeoNames.Entities;
using System;

namespace NGeoNames
{
    /// <summary>
    /// Internal utility class.
    /// </summary>
    internal static class GeoUtil
    {
        private const double RADIUSOFEARTH = 6371000;   //Radius of the earth, in METERS

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="deg">Degrees.</param>
        /// <returns>Radians.</returns>
        public static double Deg2Rad(double deg)
        {
            return (Math.PI / 180.0) * deg;
        }

        /// <summary>
        /// Calculates the distance from the source to destination location (in meters).
        /// </summary>
        /// <param name="src">The source location.</param>
        /// <param name="dest">The destination location.</param>
        /// <param name="radiusofearth">The radius of the earth in meters (default: 6371000).</param>
        /// <returns>Returns the distance, in meters, from source to destination.</returns>
        /// <remarks>
        /// Note that we use the <a href="http://en.wikipedia.org/wiki/International_System_of_Units">International
        /// System of Units (SI)</a>; units of distance are specified in meters. If you want to use imperial system (e.g.
        /// miles, nautical miles, yards, foot and whathaveyou's) you need to convert from/to meters.
        /// </remarks>
        public static double DistanceTo(IGeoLocation src, IGeoLocation dest, double radiusofearth = RADIUSOFEARTH)
        {
            double dLat = GeoUtil.Deg2Rad(dest.Latitude - src.Latitude);
            double dLon = GeoUtil.Deg2Rad(dest.Longitude - src.Longitude);
            return (radiusofearth / 1000) * (2 * Math.Asin(Math.Min(1, Math.Sqrt(Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(GeoUtil.Deg2Rad(src.Latitude)) * Math.Cos(GeoUtil.Deg2Rad(dest.Latitude)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2)))));
        }

        /// <summary>
        /// Calculates a coordinate from a GeoName (latitude, longitude, altitude)
        /// </summary>
        /// <param name="n">The GeoName to determine the coordinate for.</param>
        /// <param name="radiusofearth">Radius of the earth, in meters.</param>
        /// <returns>Returns a coordinate (represented as an array of 3 doubles).</returns>
        /// <remarks>This method is for internal use (for the KdTree) only.</remarks>
        public static double[] GetCoord(IGeoLocation n, double radiusofearth = RADIUSOFEARTH)
        {
            return new[] {
                radiusofearth * Math.Cos(GeoUtil.Deg2Rad(n.Latitude)) * Math.Cos(GeoUtil.Deg2Rad(n.Longitude)),
                radiusofearth * Math.Cos(GeoUtil.Deg2Rad(n.Latitude)) * Math.Sin(GeoUtil.Deg2Rad(n.Longitude)),
                radiusofearth * Math.Sin(GeoUtil.Deg2Rad(n.Latitude))
            };
        }
    }
}
