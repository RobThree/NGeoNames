namespace NGeoNames.Entities
{
    /// <summary>
    /// Represents a ("compact") GeoName record; this class also serves as baseclass for <see cref="ExtendedGeoName"/>.
    /// </summary>
    /// <seealso cref="ExtendedGeoName"/>
    public class GeoName
    {
        /// <summary>
        /// Gets/sets the record-ID in the geonames database.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Gets/sets the name of geographical point.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets/sets the latitude in decimal degrees.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets/sets the longitude in decimal degrees.
        /// </summary>
        public double Longitude { get; set; }


        /// <summary>
        /// Calculates the distance from this instance to destination location (in meters).
        /// </summary>
        /// <param name="lat">The latitude of the destination point.</param>
        /// <param name="lng">The longitude of the destination point.</param>
        /// <returns>Returns the distance, in meters, from this instance to destination.</returns>
        /// <remarks>
        /// Note that we use the <a href="http://en.wikipedia.org/wiki/International_System_of_Units">International
        /// System of Units (SI)</a>; units of distance are specified in meters. If you want to use imperial system (e.g.
        /// miles, nautical miles, yards, foot and other units) you need to convert from/to meters.
        /// </remarks>
        public double DistanceTo(double lat, double lng)
        {
            return GeoUtil.DistanceTo(this, new GeoName { Latitude = lat, Longitude = lng });
        }

        /// <summary>
        /// Calculates the distance from this instance to destination location (in meters).
        /// </summary>
        /// <param name="lat">The latitude of the destination point.</param>
        /// <param name="lng">The longitude of the destination point.</param>
        /// <param name="radiusofearthinmeters">The radius of the earth in meters (default: 6371000).</param>
        /// <returns>Returns the distance, in meters, from this instance to destination.</returns>
        /// <remarks>
        /// Note that we use the <a href="http://en.wikipedia.org/wiki/International_System_of_Units">International
        /// System of Units (SI)</a>; units of distance are specified in meters. If you want to use imperial system (e.g.
        /// miles, nautical miles, yards, foot and other units) you need to convert from/to meters.
        /// </remarks>
        public double DistanceTo(double lat, double lng, double radiusofearthinmeters)
        {
            return GeoUtil.DistanceTo(this, new GeoName { Latitude = lat, Longitude = lng }, radiusofearthinmeters);
        }

        /// <summary>
        /// Calculates the distance from the this instance to destination location (in meters).
        /// </summary>
        /// <param name="loc">The destination location.</param>
        /// <returns>Returns the distance, in meters, from this instance to destination.</returns>
        /// <remarks>
        /// Note that we use the <a href="http://en.wikipedia.org/wiki/International_System_of_Units">International
        /// System of Units (SI)</a>; units of distance are specified in meters. If you want to use imperial system (e.g.
        /// miles, nautical miles, yards, foot and other units) you need to convert from/to meters.
        /// </remarks>
        public double DistanceTo(GeoName loc)
        {
            return GeoUtil.DistanceTo(this, loc);
        }

        /// <summary>
        /// Calculates the distance from this instance to destination location (in meters).
        /// </summary>
        /// <param name="loc">The destination location.</param>
        /// <param name="radiusofearthinmeters">The radius of the earth in meters (default: 6371000).</param>
        /// <returns>Returns the distance, in meters, from this instance to destination.</returns>
        /// <remarks>
        /// Note that we use the <a href="http://en.wikipedia.org/wiki/International_System_of_Units">International
        /// System of Units (SI)</a>; units of distance are specified in meters. If you want to use imperial system (e.g.
        /// miles, nautical miles, yards, foot and other units) you need to convert from/to meters.
        /// </remarks>
        public double DistanceTo(GeoName loc, double radiusofearthinmeters)
        {
            return GeoUtil.DistanceTo(this, loc, radiusofearthinmeters);
        }
    }
}
