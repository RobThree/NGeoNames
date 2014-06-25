namespace NGeoNames.Entities
{
    /// <summary>
    /// Represents a ("compact") GeoName record; this class also serves as baseclass for <see cref="ExtendedGeoName"/>.
    /// </summary>
    /// <seealso cref="ExtendedGeoName"/>
    public class GeoName : IGeoLocation
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
    }
}
