using NGeoNames.Entities;
using System;
using System.Globalization;

namespace NGeoNames.Parsers
{
    public class CountryInfoParser : BaseParser<CountryInfo>
    {
        private static readonly char[] csv = { ',' };

        public override bool HasComments
        {
            get { return true; }
        }

        public override int SkipLines
        {
            get { return 0; }
        }

        public override int ExpectedNumberOfFields
        {
            get { return 19; }
        }

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
                Phone = (fields[12] ?? string.Empty).Length > 0 && (fields[12] ?? string.Empty).StartsWith("+") ? fields[12] : "+" + fields[12],
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
