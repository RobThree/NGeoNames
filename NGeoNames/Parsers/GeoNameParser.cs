using NGeoNames.Entities;
using System;
using System.Globalization;

namespace NGeoNames.Parsers
{
    public class GeoNameParser : BaseParser<GeoName>
    {
        private static readonly char[] csv = { ',' };

        private int _expectednumberoffields;

        public GeoNameParser()
            : this(true) { }

        public GeoNameParser(bool useextendedfileformat)
        {
            _expectednumberoffields = useextendedfileformat ? 19 : 4;
        }

        public override bool HasComments
        {
            get { return false; }
        }

        public override int SkipLines
        {
            get { return 0; }
        }

        public override int ExpectedNumberOfFields { get { return _expectednumberoffields; } }

        public override GeoName Parse(string[] fields)
        {
            switch (this.ExpectedNumberOfFields)
            {
                case 4:
                    return new GeoName
                    {
                        Id = int.Parse(fields[0]),
                        Name = fields[1],
                        Latitude = double.Parse(fields[2], CultureInfo.InvariantCulture),
                        Longitude = double.Parse(fields[3], CultureInfo.InvariantCulture),
                    };
                case 19:
                    return new GeoName
                    {
                        Id = int.Parse(fields[0]),
                        Name = fields[1],
                        Latitude = double.Parse(fields[4], CultureInfo.InvariantCulture),
                        Longitude = double.Parse(fields[5], CultureInfo.InvariantCulture),
                    };
            }
            throw new NotSupportedException(string.Format("Unsupported number of fields: {0}", this.ExpectedNumberOfFields));
        }

    }
}
