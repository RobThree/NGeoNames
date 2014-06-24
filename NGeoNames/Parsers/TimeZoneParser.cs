using NGeoNames.Entities;
using System.Globalization;

namespace NGeoNames.Parsers
{
    /// <summary>
    /// Provides methods for parsing a <see cref="TimeZone"/> object from a string-array.
    /// </summary>
    public class TimeZoneParser : BaseParser<TimeZone>
    {
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
            get { return 1; }
        }

        /// <summary>
        /// Gets the number of fields the file/stream is expected to have; anything else will cause a <see cref="ParserException"/>.
        /// </summary>
        public override int ExpectedNumberOfFields
        {
            get { return 5; }
        }

        /// <summary>
        /// Parses the specified data into a <see cref="TimeZone"/> object.
        /// </summary>
        /// <param name="fields">The fields/data representing a <see cref="TimeZone"/> to parse.</param>
        /// <returns>Returns a new <see cref="TimeZone"/> object.</returns>
        public override TimeZone Parse(string[] fields)
        {
            return new TimeZone
            {
                CountryCode = fields[0],
                TimeZoneId = fields[1].Replace("_", " "),
                GMTOffset = float.Parse(fields[2], CultureInfo.InvariantCulture),
                DSTOffset = float.Parse(fields[3], CultureInfo.InvariantCulture),
                RawOffset = float.Parse(fields[4], CultureInfo.InvariantCulture)
            };
        }
    }
}
