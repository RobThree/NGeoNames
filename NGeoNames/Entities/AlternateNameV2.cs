namespace NGeoNames.Entities
{
    /// <summary>
    /// Represents an alternate name for a specific <see cref="GeoName"/> or <see cref="ExtendedGeoName"/>. This entity
    /// represents a newer version of the <see cref="AlternateName"/> type, with new From and To properties that describe
    /// the historical usage of the name.
    /// </summary>
    /// <remarks>
    /// The property <see cref="ExtendedGeoName.AlternateNames"/> of the <see cref="ExtendedGeoName"/> object is a short
    /// version of the <see cref="AlternateNameV2"/> data without links and postal codes but with ASCUU transliterations.
    /// </remarks>
    public class AlternateNameV2 : AlternateName
    {
        /// <summary>
        /// Gets/sets the start date for the historical period during which this name was used.
        /// </summary>
        /// <remarks>
        /// Note that this property is a string and may contain all sorts of 'data' like "1938-10-03", 
        /// "19 February 2008", "201806", "397", "قولەی کانی ماران" and even links to wikipedia.
        /// </remarks>
        public string From { get; set; }

        /// <summary>
        /// Gets/sets the end date for the historical period during which this name was used.
        /// </summary>
        /// <remarks>
        /// Note that this property is a string and may contain all sorts of 'data' like "1938-10-03", 
        /// "19 February 2008", "201806", "397", "قولەی کانی ماران" and even links to wikipedia.
        /// </remarks>
        public string To { get; set; }
    }
}