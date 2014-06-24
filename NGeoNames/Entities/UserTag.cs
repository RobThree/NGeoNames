namespace NGeoNames.Entities
{
    /// <summary>
    /// Represents information about a geoname.org entity.
    /// </summary>
    public class UserTag
    {
        /// <summary>
        /// Gets/sets the geoname database Id the <see cref="UserTag"/> is referring to.
        /// </summary>
        public int GeoNameId { get; set; }

        /// <summary>
        /// Gets/sets the value of the <see cref="UserTag"/>.
        /// </summary>
        public string Tag { get; set; }
    }
}
