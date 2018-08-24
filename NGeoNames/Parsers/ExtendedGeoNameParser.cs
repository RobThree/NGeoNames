using NGeoNames.Entities;
using System;
using System.Globalization;

namespace NGeoNames.Parsers
{
    /// <summary>
    /// Provides methods for parsing an <see cref="ExtendedGeoName"/> object from a string-array.
    /// </summary>
    public class ExtendedGeoNameParser : BaseParser<ExtendedGeoName>
    {
        /// <summary>
        /// Gets wether the file/stream has (or is expected to have) comments (lines starting with "#").
        /// </summary>
        public override bool HasComments => false;

        /// <summary>
        /// Gets the number of lines to skip when parsing the file/stream (e.g. 'headers' etc.).
        /// </summary>
        public override int SkipLines => 0;

        /// <summary>
        /// Gets the number of fields the file/stream is expected to have; anything else will cause a <see cref="ParserException"/>.
        /// </summary>
        public override int ExpectedNumberOfFields => 19;

        /// <summary>
        /// Parses the specified data into an <see cref="ExtendedGeoName"/> object.
        /// </summary>
        /// <param name="fields">The fields/data representing an <see cref="ExtendedGeoName"/> to parse.</param>
        /// <returns>Returns a new <see cref="ExtendedGeoName"/> object.</returns>
        public override ExtendedGeoName Parse(string[] fields)
        {
            return new ExtendedGeoName
            {
                Id = StringToInt(fields[0]),
                Name = fields[1],
                NameASCII = fields[2],
                AlternateNames = StringToArray(fields[3]),
                Latitude = StringToDouble(fields[4]),
                Longitude = StringToDouble(fields[5]),
                FeatureClass = fields[6],
                FeatureCode = fields[7],
                CountryCode = fields[8],
                AlternateCountryCodes = StringToArray(fields[9]),
                Admincodes = new[] { fields[10], fields[11], fields[12], fields[13] },
                Population = StringToLong(fields[14]),
                Elevation = fields[15].Length > 0 ? (int?)StringToInt(fields[15]) : null,
                Dem = StringToInt(fields[16]),
                Timezone = StringToTimeZone(fields[17]),
                ModificationDate = StringToDateTime(fields[18])
            };
        }
    }
}
