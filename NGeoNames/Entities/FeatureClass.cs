namespace NGeoNames.Entities
{
    /// <summary>
    /// Represents a feature class.
    /// </summary>
    /// <remarks>See <a href="http://www.geonames.org/export/codes.html">http://www.geonames.org/export/codes.html</a>.</remarks>
    public class FeatureClass
    {
        /// <summary>
        /// Gets/sets the class "Id" (or "code) of the <see cref="FeatureClass"/>.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Gets/sets the description of the <see cref="FeatureClass"/>.
        /// </summary>
        public string Description { get; set; }
    }
}
