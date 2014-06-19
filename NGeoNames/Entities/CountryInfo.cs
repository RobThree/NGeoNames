namespace NGeoNames.Entities
{
    public class CountryInfo
    {

        public string ISO_Alpha2 { get; set; }
        public string ISO_Alpha3 { get; set; }
        public string ISO_Numeric { get; set; }
        /// <summary>
        /// Federal Information Processing Standards
        /// </summary>
        public string FIPS { get; set; }
        public string Country { get; set; }
        public string Capital { get; set; }
        /// <summary>
        /// Area in km²
        /// </summary>
        public float? Area { get; set; }
        public int Population { get; set; }
        public string Continent { get; set; }
        public string Tld { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string Phone { get; set; }
        public string PostalCodeFormat { get; set; }
        public string PostalCodeRegex { get; set; }
        public string[] Languages { get; set; }
        public int? GeoNameId { get; set; }
        public string[] Neighbours { get; set; }
        public string EquivalentFipsCode { get; set; }
    }
}
