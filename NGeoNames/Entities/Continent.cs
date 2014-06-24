namespace NGeoNames.Entities
{
    /// <summary>
    /// Represents a continent.
    /// </summary>
    public class Continent
    {
        /// <summary>
        /// Gets/sets the <see cref="Continent"/> code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets/sets the <see cref="Continent"/> name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets/sets the <see cref="Continent"/> geoname database Id.
        /// </summary>
        public int GeoNameId { get; set; }
    }
}
