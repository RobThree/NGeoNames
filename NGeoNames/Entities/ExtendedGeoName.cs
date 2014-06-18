using System;

namespace NGeoNames.Entities
{
    public class ExtendedGeoName : GeoName
    {
        public string ASCIIName { get; set; }
        /// <summary>
        /// Alternatenames, ascii names automatically transliterated, convenience attribute from alternatename table, varchar(8000)
        /// </summary>
        public string[] AlternateNames { get; set; }
        /// <summary>
        /// See http://www.geonames.org/export/codes.html, char(1)
        /// </summary>
        public string FeatureClass { get; set; }
        /// <summary>
        /// See http://www.geonames.org/export/codes.html, varchar(10)
        /// </summary>
        public string FeatureCode { get; set; }
        /// <summary>
        /// ISO-3166 2-letter country code, 2 characters
        /// </summary>
        public string CountryCode { get; set; }
        /// <summary>
        /// Alternate country codes, ISO-3166 2-letter country code, 60 characters
        /// </summary>
        public string[] AlternateCountryCodes { get; set; }
        /// <summary>
        /// Admincode[0]: fipscode (subject to change to iso code), see exceptions below, see file admin1Codes.txt for display names of this code; varchar(20)
        /// Admincode[1]: code for the second administrative division, a county in the US, see file admin2Codes.txt; varchar(80) 
        /// Admincode[2]: code for third level administrative division, varchar(20)
        /// Admincode[3]: code for fourth level administrative division, varchar(20)
        /// </summary>
        public string[] Admincodes { get; set; }
        /// <summary>
        /// Bigint (8 byte int) 
        /// </summary>
        public long Population { get; set; }
        /// <summary>
        /// In meters, integer
        /// </summary>
        public int? Elevation { get; set; }
        /// <summary>
        /// Digital elevation model, srtm3 or gtopo30, average elevation of 3''x3'' (ca 90mx90m) or 30''x30'' (ca 900mx900m) area in meters, integer. srtm processed by cgiar/ciat.
        /// </summary>
        public int Dem { get; set; }
        /// <summary>
        /// The timezone id (see file timeZone.txt) varchar(40)
        /// </summary>
        public string Timezone { get; set; }
        /// <summary>
        /// Date of last modification in yyyy-MM-dd format
        /// </summary>
        public DateTime ModificationDate { get; set; }
    }
}
