namespace NGeoNames.Entities
{
    /// <summary>
    /// Represents an administrative division.
    /// </summary>
    public class Admin1Code
    {
        /// <summary>
        /// Gets/sets the code of the administrative division.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets/sets the name of the administrative division.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets/sets the name of the administrative division in plain ASCII.
        /// </summary>
        /// <remarks>
        /// Non-ASCII values have been found in the data; it is unfortunately (currently) *NOT* guaranteed that this
        /// property contains ASCII-only strings.
        /// </remarks>
        public string NameASCII { get; set; }

        /// <summary>
        /// Gets/sets the geoname database Id of the administrative division.
        /// </summary>
        public int GeoNameId { get; set; }
    }
}
