namespace NGeoNames.Entities
{
    /// <summary>
    /// Represents an postal code.
    /// </summary>
    public class Postalcode : IGeoLocation
    {
        /// <summary>
        /// Gets/sets the ISO-3166 2-letter country code of the <see cref="Postalcode"/>.
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets/sets the postal code.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets/sets the name of the place.
        /// </summary>
        public string PlaceName { get; set; }

        /// <summary>
        /// Gets/sets 1 up to 3 admin names (indexed 0 to 2) of the <see cref="Postalcode"/>.
        /// <list type="bullet">
        ///     <item>AdminName[0]: 1st order subdivision (state).</item>
        ///     <item>AdminName[1]: 2nd order subdivision (county/province).</item>
        ///     <item>AdminName[2]: 3rd order subdivision (community).</item>
        /// </list>
        /// </summary>
        public string[] AdminName { get; set; }

        /// <summary>
        /// Gets/sets 1 up to 3 admin codes (indexed 0 to 2) of the <see cref="Postalcode"/>.
        /// <list type="bullet">
        ///     <item>AdminCode[0]: 1st order subdivision (state).</item>
        ///     <item>AdminCode[1]: 2nd order subdivision (county/province).</item>
        ///     <item>AdminCode[2]: 3rd order subdivision (community).</item>
        /// </list>
        /// </summary>
        public string[] AdminCode { get; set; }

        /// <summary>
        /// Gets/sets the latitude in decimal degrees.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets/sets the longitude in decimal degrees.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Gets/sets the accuracy of <see cref="Latitude"/>/<see cref="Longitude"/> from 1 = estimated to 6 = centroid.
        /// </summary>
        public int? Accuracy { get; set; }
    }
}
