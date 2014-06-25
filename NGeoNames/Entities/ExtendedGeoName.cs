using System;

namespace NGeoNames.Entities
{
    /// <summary>
    /// Represents a GeoName record with all data represented.
    /// </summary>
    /// <seealso cref="GeoName"/>
    public class ExtendedGeoName : GeoName
    {
        /// <summary>
        /// Gets/sets the name of the <see cref="ExtendedGeoName"/>.
        /// </summary>
        /// <remarks>
        /// Non-ASCII values have been found in the data; it is unfortunately (currently) *NOT* guaranteed that this
        /// property contains ASCII-only strings.
        /// </remarks>
        public string NameASCII { get; set; }

        /// <summary>
        /// Gets/sets the alternatenames / ASCII names of the <see cref="ExtendedGeoName"/>.
        /// </summary>
        /// <remarks>See <see cref="AlternateName"/>.</remarks>
        public string[] AlternateNames { get; set; }

        /// <summary>
        /// Gets/sets the <see cref="FeatureClass"/> code of the <see cref="ExtendedGeoName"/>.
        /// </summary>
        public string FeatureClass { get; set; }

        /// <summary>
        /// Gets/sets the <see cref="FeatureCode"/> code of the <see cref="ExtendedGeoName"/>.
        /// </summary>
        public string FeatureCode { get; set; }

        /// <summary>
        /// Gets/sets the ISO-3166 2-letter country code of the <see cref="ExtendedGeoName"/>.
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets/sets the alternate ISO-3166 2-letter country codes.
        /// </summary>
        public string[] AlternateCountryCodes { get; set; }

        private string[] _admincodes;
        /// <summary>
        /// Gets/sets 1 up to 4 admin codes (indexed 0 to 3) of the <see cref="ExtendedGeoName"/>.
        /// <list type="bullet">
        ///     <item>Admincode[0]: FIPS code (subject to change to iso code), see remarks and <see cref="Admin1Code"/>.</item>
        ///     <item>Admincode[1]: code for the second administrative division (a county in the US). See <see cref="Admin2Code"/>.</item>
        ///     <item>Admincode[2]: code for third level administrative division.</item>
        ///     <item>Admincode[3]: code for fourth level administrative division.</item>
        /// </list>
        /// </summary>
        /// <remarks>
        /// Most Admincode[0] codes are FIPS codes. ISO codes are used for US, CH, BE and ME. UK and Greece are using
        /// an additional level between country and FIPS code. The code '00' stands for general features where no
        /// specific Admincode[0] code is defined.
        /// </remarks>
        public string[] Admincodes {
            get { return _admincodes; }
            set
            {
                if (value.Length != 4)
                    throw new ArgumentOutOfRangeException("Admincodes array must be of length 4");
                _admincodes = value;
            }
        }

        /// <summary>
        /// Gets/sets the population of the <see cref="ExtendedGeoName"/>.
        /// </summary>
        public long Population { get; set; }

        /// <summary>
        /// Gets/sets the elevation (in meters) of the <see cref="ExtendedGeoName"/>.
        /// </summary>
        public int? Elevation { get; set; }

        /// <summary>
        /// Gets/sets the Digital Elevation Model, srtm3 or gtopo30, average elevation of 3''x3'' (ca 90mx90m) or 
        /// 30''x30'' (ca 900mx900m) area in meters of the <see cref="ExtendedGeoName"/>.
        /// </summary>
        /// <remarks>
        /// srtm processed by cgiar/ciat
        /// </remarks>
        public int Dem { get; set; }

        /// <summary>
        /// Gets/sets the <see cref="TimeZone"/> id of the <see cref="ExtendedGeoName"/>.
        /// </summary>
        public string Timezone { get; set; }

        /// <summary>
        /// Gets/sets the date of last modification of the <see cref="ExtendedGeoName"/>.
        /// </summary>
        public DateTime ModificationDate { get; set; }
    }
}
