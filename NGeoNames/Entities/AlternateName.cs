namespace NGeoNames.Entities
{
    /// <summary>
    /// Represents an alternate name for a specific <see cref="GeoName"/> or <see cref="ExtendedGeoName"/>.
    /// </summary>
    /// <remarks>
    /// The property <see cref="ExtendedGeoName.AlternateNames"/> of the <see cref="ExtendedGeoName"/> object is a short
    /// version of the <see cref="AlternateName"/> data without links and postal codes but with ASCUU transliterations.
    /// </remarks>
    public class AlternateName
    {
        /// <summary>
        /// Gets/sets the Id of the <see cref="AlternateName"/>.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets/sets the geoname database Id of the <see cref="GeoName"/> or <see cref="ExtendedGeoName"/> the
        /// <see cref="AlternateName"/> refers to.
        /// </summary>
        public int GeoNameId { get; set; }

        /// <summary>
        /// Gets/sets the ISO 639 language code 2- or 3-characters.
        /// </summary>
        public string ISOLanguage { get; set; }

        /// <summary>
        /// Gets/sets the type of the <see cref="AlternateName"/>; this can be of, but is not limited to, type 'post'
        /// for postal codes, 'iata', 'icao' and 'faac' for airport codes, 'fr_1793' for French Revolution names, 
        /// 'abbr' for abbreviation and 'link' for a website.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets/sets the <see cref="AlternateName"/> or name variant or the value of the specified <see cref="Type"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets/sets whether this <see cref="AlternateName"/> is an official/preferred name.
        /// </summary>
        public bool IsPreferredName { get; set; }

        /// <summary>
        /// Gets/sets whether this <see cref="AlternateName"/> is a short name like 'California' for 'State of California'.
        /// </summary>
        public bool IsShortName { get; set; }

        /// <summary>
        /// Gets/sets whether this <see cref="AlternateName"/> is a colloquial or slang term.
        /// </summary>
        public bool IsColloquial { get; set; }

        /// <summary>
        /// Gets/sets whether this <see cref="AlternateName"/> is historic and was used in the past
        /// </summary>
        public bool IsHistoric { get; set; }
    }
}
