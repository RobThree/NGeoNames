using NGeoNames.Entities;
using System.Globalization;

namespace NGeoNames.Parsers
{
    public class TimeZoneParser : BaseParser<TimeZone>
    {
        public override bool HasComments
        {
            get { return false; }
        }

        public override int SkipLines
        {
            get { return 1; }
        }

        public override int ExpectedNumberOfFields
        {
            get { return 5; }
        }


        public override TimeZone Parse(string[] fields)
        {
            return new TimeZone
            {
                CountryCode = fields[0],
                TimeZoneId = fields[1],
                GMTOffset = float.Parse(fields[2], CultureInfo.InvariantCulture),
                DSTOffset = float.Parse(fields[3], CultureInfo.InvariantCulture),
                RawOffset = float.Parse(fields[4], CultureInfo.InvariantCulture)
            };
        }
    }
}
