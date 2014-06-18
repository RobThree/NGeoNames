using NGeoNames.Entities;
using System;
using System.Globalization;

namespace NGeoNames.Parsers
{
    public class ExtendedGeoNameParser : BaseParser<ExtendedGeoName>
    {
        private static readonly char[] csv = { ',' };

        public override bool HasComments
        {
            get { return false; }
        }

        public override int SkipLines
        {
            get { return 0; }
        }

        public override int ExpectedNumberOfFields
        {
            get { return 19; }
        }
        
        public override ExtendedGeoName Parse(string[] fields)
        {
            return new ExtendedGeoName
            {
                Id = int.Parse(fields[0]),
                Name = fields[1],
                ASCIIName = fields[2],
                AlternateNames = fields[3].Split(csv, StringSplitOptions.RemoveEmptyEntries),
                Latitude = double.Parse(fields[4], CultureInfo.InvariantCulture),
                Longitude = double.Parse(fields[5], CultureInfo.InvariantCulture),
                FeatureClass = fields[6],
                FeatureCode = fields[7],
                CountryCode = fields[8],
                AlternateCountryCodes = fields[9].Split(csv, StringSplitOptions.RemoveEmptyEntries),
                Admincodes = new[] { fields[10], fields[11], fields[12], fields[13] },
                Population = long.Parse(fields[14]),
                Elevation = fields[15].Length > 0 ? (int?)int.Parse(fields[15]) : null,
                Dem = int.Parse(fields[16]),
                Timezone = fields[17].Replace("_", " "),
                ModificationDate = DateTime.ParseExact(fields[18], "yyyy-MM-dd", CultureInfo.InvariantCulture)
            };
        }
    }
}
