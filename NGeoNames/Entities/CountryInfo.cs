namespace NGeoNames.Entities
{
    /// <summary>
    /// Represents information about a country.
    /// </summary>
    public class CountryInfo
    {
        /// <summary>
        /// Gets/sets the ISO-3166 2-letter country code.
        /// </summary>
        public string ISO_Alpha2 { get; set; }

        /// <summary>
        /// Gets/sets the ISO-3166 3-letter country code.
        /// </summary>
        public string ISO_Alpha3 { get; set; }

        /// <summary>
        /// Gets/sets the ISO-3166 numeric country code.
        /// </summary>
        public string ISO_Numeric { get; set; }

        /// <summary>
        /// Gets/sets the Federal Information Processing Standards code.
        /// </summary>
        public string FIPS { get; set; }

        /// <summary>
        /// Gets/sets the name of the country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets/sets the capital of the country.
        /// </summary>
        public string Capital { get; set; }

        /// <summary>
        /// Gets/sets the area in km² of the country.
        /// </summary>
        public float? Area { get; set; }

        /// <summary>
        /// Gets/sets the population of the country.
        /// </summary>
        public int Population { get; set; }

        /// <summary>
        /// Gets/sets the <see cref="Continent"/> code of the country.
        /// </summary>
        public string Continent { get; set; }

        /// <summary>
        /// Gets/sets the TLD (Top-Level Domain) of the country.
        /// </summary>
        public string Tld { get; set; }

        /// <summary>
        /// Gets/sets the currency code of the country.
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets/sets the currency name of the country.
        /// </summary>
        public string CurrencyName { get; set; }

        /// <summary>
        /// Gets/sets the international dialing code of the country.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets/sets the postal code format of the country.
        /// </summary>
        public string PostalCodeFormat { get; set; }

        /// <summary>
        /// Gets/sets the regex to match a postal code of the country.
        /// </summary>
        public string PostalCodeRegex { get; set; }

        /// <summary>
        /// Gets/sets the associated language(s) of the country.
        /// </summary>
        public string[] Languages { get; set; }

        /// <summary>
        /// Gets/sets the <see cref="CountryInfo"/>'s geoname database Id.
        /// </summary>
        public int? GeoNameId { get; set; }

        /// <summary>
        /// Gets/sets the ISO-3166 2-letter country code(s) of the countries neighbouring the country.
        /// </summary>
        public string[] Neighbours { get; set; }

        /// <summary>
        /// Gets/sets the Equivalent FIPS code of the country.
        /// </summary>
        /// <remarks>
        ///  A country (as defined by ISO) might not have any associated FIPS Code. It also happens that several ISO
        ///  countries have the same FIPS code (e.g. : Finland, and aalen island). In these cases, Finland would have
        ///  the original FIPS code, and aalen island would have this code as its equivalent FIPS code
        /// </remarks>
        public string EquivalentFipsCode { get; set; }
    }
}
