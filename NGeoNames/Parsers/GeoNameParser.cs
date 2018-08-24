using NGeoNames.Entities;
using System;
using System.Globalization;

namespace NGeoNames.Parsers
{
    /// <summary>
    /// Provides methods for parsing a <see cref="GeoName"/> object from a string-array.
    /// </summary>
    public class GeoNameParser : BaseParser<GeoName>
    {
        private static readonly char[] csv = { ',' };

        private int _expectednumberoffields;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoNameParser"/> class with default values (extended fileformat, 19 fields).
        /// </summary>
        public GeoNameParser()
            : this(true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoNameParser"/> class with specified file format.
        /// </summary>
        /// <param name="useextendedfileformat">
        /// When this parameter is true, the (default) file format (19 fields) will be assumed for geoname data,
        /// when this parameter is false, the "compact file format" (4 fields: Id, Name, Latitude and Longitude)
        /// will be assumed.
        /// </param>
        public GeoNameParser(bool useextendedfileformat)
        {
            _expectednumberoffields = useextendedfileformat ? 19 : 4;
        }

        /// <summary>
        /// Gets wether the file/stream has (or is expected to have) comments (lines starting with "#").
        /// </summary>
        public override bool HasComments
        {
            get { return false; }
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
        public override int ExpectedNumberOfFields { get { return _expectednumberoffields; } }

        /// <summary>
        /// Parses the specified data into a <see cref="GeoName"/> object.
        /// </summary>
        /// <param name="fields">The fields/data representing a <see cref="GeoName"/> to parse.</param>
        /// <returns>Returns a new <see cref="GeoName"/> object.</returns>
        public override GeoName Parse(string[] fields)
        {
            switch (ExpectedNumberOfFields)
            {
                case 4:
                    return new GeoName
                    {
                        Id = StringToInt(fields[0]),
                        Name = fields[1],
                        Latitude = StringToDouble(fields[2]),
                        Longitude = StringToDouble(fields[3]),
                    };
                case 19:
                    return new GeoName
                    {
                        Id = StringToInt(fields[0]),
                        Name = fields[1],
                        Latitude = StringToDouble(fields[4]),
                        Longitude = StringToDouble(fields[5]),
                    };
            }
            throw new NotSupportedException(string.Format("Unsupported number of fields: {0}", ExpectedNumberOfFields));
        }

    }
}
