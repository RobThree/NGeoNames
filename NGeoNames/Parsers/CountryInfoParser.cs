using NGeoNames.Entities;
using System;
using System.Globalization;

namespace NGeoNames.Parsers
{
    /// <summary>
    /// Provides methods for parsing a <see cref="CountryInfo"/> object from a string-array.
    /// </summary>
    public class CountryInfoParser : BaseParser<CountryInfo>
    {
        private static readonly char[] csv = { ',' };

        /// <summary>
        /// Gets wether the file/stream has (or is expected to have) comments (lines starting with "#").
        /// </summary>
        public override bool HasComments
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the number of lines to skip when parsing the file/stream (e.g. 'headers' etc.).
        /// </summary>
        public override int SkipLines
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets the number of fields the file/stream is expected to have; anything else will cause a <see cref="ParserException"/>.
        /// </summary>
        public override int ExpectedNumberOfFields
        {
            get { return 19; }
        }

        /// <summary>
        /// Parses the specified data into a <see cref="CountryInfo"/> object.
        /// </summary>
        /// <param name="fields">The fields/data representing a <see cref="CountryInfo"/> to parse.</param>
        /// <returns>Returns a new <see cref="CountryInfo"/> object.</returns>
        public override CountryInfo Parse(string[] fields)
        {
            return new CountryInfo
            {
                ISO_Alpha2 = fields[0],
                ISO_Alpha3 = fields[1],
                ISO_Numeric = fields[2],
                FIPS = fields[3],
                Country = fields[4],
                Capital = fields[5],
                Area = fields[6].Length > 0 ? (float?)float.Parse(fields[6], CultureInfo.InvariantCulture) : null,
                Population = int.Parse(fields[7]),
                Continent = fields[8],
                Tld = fields[9],
                CurrencyCode = fields[10],
                CurrencyName = fields[11],
                Phone = fields[12].Length > 0 && fields[12].StartsWith("+") ? fields[12] : "+" + fields[12],
                PostalCodeFormat = fields[13],
                PostalCodeRegex = fields[14],
                Languages = fields[15].Split(csv, StringSplitOptions.RemoveEmptyEntries),
                GeoNameId = fields[16].Length > 0 ? (int?)int.Parse(fields[16], CultureInfo.InvariantCulture) : null,
                Neighbours = fields[17].Split(csv, StringSplitOptions.RemoveEmptyEntries),
                EquivalentFipsCode = fields[18]
            };
        }
    }
}
