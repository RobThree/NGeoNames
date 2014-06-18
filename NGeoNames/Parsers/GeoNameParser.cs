using NGeoNames.Entities;
using System.Globalization;

namespace NGeoNames.Parsers
{
    public class GeoNameParser : BaseParser<GeoName>
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

        public override GeoName Parse(string[] fields)
        {
            return new GeoName
            {
                Id = int.Parse(fields[0]),
                Name = fields[1],
                Latitude = double.Parse(fields[4], CultureInfo.InvariantCulture),
                Longitude = double.Parse(fields[5], CultureInfo.InvariantCulture),
            };
        }
    }
}
