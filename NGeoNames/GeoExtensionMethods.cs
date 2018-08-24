namespace NGeoNames.Entities
{
    /// <summary>
    /// Provides geolocation related extension methods.
    /// </summary>
    public static class GeoExtensionMethods
    {
        /// <summary>
        /// Calculates the distance from this instance to destination location (in meters).
        /// </summary>
        /// <param name="loc">The <see cref="IGeoLocation"/> to apply the method to.</param>
        /// <param name="lat">The latitude of the destination point.</param>
        /// <param name="lng">The longitude of the destination point.</param>
        /// <returns>Returns the distance, in meters, from this instance to destination.</returns>
        /// <remarks>
        /// Note that we use the <a href="http://en.wikipedia.org/wiki/International_System_of_Units">International
        /// System of Units (SI)</a>; units of distance are specified in meters. If you want to use imperial system (e.g.
        /// miles, nautical miles, yards, foot and other units) you need to convert from/to meters. You can use the
        /// helper methods <see cref="GeoUtil.MilesToMeters"/> / <see cref="GeoUtil.MetersToMiles"/> and 
        /// <see cref="GeoUtil.YardsToMeters"/> / <see cref="GeoUtil.MetersToYards"/> for quick conversion.
        /// </remarks>
        /// <seealso cref="GeoUtil.MilesToMeters"/>
        /// <seealso cref="GeoUtil.MetersToMiles"/>
        /// <seealso cref="GeoUtil.YardsToMeters"/>
        /// <seealso cref="GeoUtil.MetersToYards"/>
        public static double DistanceTo(this IGeoLocation loc, double lat, double lng)
        {
            return GeoUtil.DistanceTo(loc, new GeoName { Latitude = lat, Longitude = lng });
        }

        /// <summary>
        /// Calculates the distance from this instance to destination location (in meters).
        /// </summary>
        /// <param name="loc">The <see cref="IGeoLocation"/> to apply the method to.</param>
        /// <param name="lat">The latitude of the destination point.</param>
        /// <param name="lng">The longitude of the destination point.</param>
        /// <param name="radiusofearthinmeters">The radius of the earth in meters (default: 6371000).</param>
        /// <returns>Returns the distance, in meters, from this instance to destination.</returns>
        /// <remarks>
        /// Note that we use the <a href="http://en.wikipedia.org/wiki/International_System_of_Units">International
        /// System of Units (SI)</a>; units of distance are specified in meters. If you want to use imperial system (e.g.
        /// miles, nautical miles, yards, foot and other units) you need to convert from/to meters. You can use the
        /// helper methods <see cref="GeoUtil.MilesToMeters"/> / <see cref="GeoUtil.MetersToMiles"/> and 
        /// <see cref="GeoUtil.YardsToMeters"/> / <see cref="GeoUtil.MetersToYards"/> for quick conversion.
        /// </remarks>
        /// <seealso cref="GeoUtil.MilesToMeters"/>
        /// <seealso cref="GeoUtil.MetersToMiles"/>
        /// <seealso cref="GeoUtil.YardsToMeters"/>
        /// <seealso cref="GeoUtil.MetersToYards"/>
        public static double DistanceTo(this IGeoLocation loc, double lat, double lng, double radiusofearthinmeters)
        {
            return GeoUtil.DistanceTo(loc, new GeoName { Latitude = lat, Longitude = lng }, radiusofearthinmeters);
        }

        /// <summary>
        /// Calculates the distance from the this instance to destination location (in meters).
        /// </summary>
        /// <param name="src">The <see cref="IGeoLocation"/> to apply the method to.</param>
        /// <param name="dst">The destination location.</param>
        /// <returns>Returns the distance, in meters, from this instance to destination.</returns>
        /// <remarks>
        /// Note that we use the <a href="http://en.wikipedia.org/wiki/International_System_of_Units">International
        /// System of Units (SI)</a>; units of distance are specified in meters. If you want to use imperial system (e.g.
        /// miles, nautical miles, yards, foot and other units) you need to convert from/to meters. You can use the
        /// helper methods <see cref="GeoUtil.MilesToMeters"/> / <see cref="GeoUtil.MetersToMiles"/> and 
        /// <see cref="GeoUtil.YardsToMeters"/> / <see cref="GeoUtil.MetersToYards"/> for quick conversion.
        /// </remarks>
        /// <seealso cref="GeoUtil.MilesToMeters"/>
        /// <seealso cref="GeoUtil.MetersToMiles"/>
        /// <seealso cref="GeoUtil.YardsToMeters"/>
        /// <seealso cref="GeoUtil.MetersToYards"/>
        public static double DistanceTo(this IGeoLocation src, IGeoLocation dst)
        {
            return GeoUtil.DistanceTo(src, dst);
        }

        /// <summary>
        /// Calculates the distance from this instance to destination location (in meters).
        /// </summary>
        /// <param name="src">The <see cref="IGeoLocation"/> to apply the method to.</param>
        /// <param name="dst">The destination location.</param>
        /// <param name="radiusofearthinmeters">The radius of the earth in meters (default: 6371000).</param>
        /// <returns>Returns the distance, in meters, from this instance to destination.</returns>
        /// <remarks>
        /// Note that we use the <a href="http://en.wikipedia.org/wiki/International_System_of_Units">International
        /// System of Units (SI)</a>; units of distance are specified in meters. If you want to use imperial system (e.g.
        /// miles, nautical miles, yards, foot and other units) you need to convert from/to meters. You can use the
        /// helper methods <see cref="GeoUtil.MilesToMeters"/> / <see cref="GeoUtil.MetersToMiles"/> and 
        /// <see cref="GeoUtil.YardsToMeters"/> / <see cref="GeoUtil.MetersToYards"/> for quick conversion.
        /// </remarks>
        /// <seealso cref="GeoUtil.MilesToMeters"/>
        /// <seealso cref="GeoUtil.MetersToMiles"/>
        /// <seealso cref="GeoUtil.YardsToMeters"/>
        /// <seealso cref="GeoUtil.MetersToYards"/>
        public static double DistanceTo(this IGeoLocation src, IGeoLocation dst, double radiusofearthinmeters)
        {
            return GeoUtil.DistanceTo(src, dst, radiusofearthinmeters);
        }
    }
}
