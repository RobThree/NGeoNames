namespace NGeoNames.Entities
{
    /// <summary>
    /// Represents an administrative subdivision.
    /// </summary>
    public class Admin2Code
    {
        /// <summary>
        /// Gets/sets the code of the administrative subdivision.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets/sets the name of the administrative subdivision.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets/sets the name of the administrative subdivision in plain ASCII.
        /// </summary>
        /// <remarks>
        /// Non-ASCII values have been found in the data; it is unfortunately (currently) *NOT* guaranteed that this
        /// property contains ASCII-only strings.
        /// </remarks>
        public string NameASCII { get; set; }


        /// <summary>
        /// Gets/sets the geoname database Id of the administrative subdivision.
        /// </summary>
        public int GeoNameId { get; set; }
    }
}
