namespace NGeoNames.Entities
{
    /// <summary>
    /// Represents a feature code.
    /// </summary>
    /// <remarks>See <a href="http://www.geonames.org/export/codes.html">http://www.geonames.org/export/codes.html</a>.</remarks>
    public class FeatureCode
    {
        /// <summary>
        /// Gets/sets the <see cref="FeatureClass"/> this <see cref="FeatureCode"/> belongs to.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Gets/sets the code of the <see cref="FeatureCode"/>.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets/sets the name of the <see cref="FeatureCode"/>.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Gets/sets the description of the <see cref="FeatureCode"/>.
        /// </summary>
        public string Description { get; set; }
    }
}
