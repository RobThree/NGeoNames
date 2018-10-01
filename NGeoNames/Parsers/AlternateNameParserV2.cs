using NGeoNames.Entities;

namespace NGeoNames.Parsers
{
    /// <summary>
    /// Provides methods for parsing an <see cref="AlternateNameV2"/> object from a string-array.
    /// </summary>
    public class AlternateNameParserV2 : BaseParser<AlternateNameV2>
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
            get { return 0; }
        }

        /// <summary>
        /// Gets the number of fields the file/stream is expected to have; anything else will cause a <see cref="ParserException"/>.
        /// </summary>
        public override int ExpectedNumberOfFields
        {
            get { return 10; }
        }

        /// <summary>
        /// Parses the specified data into an <see cref="AlternateNameV2"/> object.
        /// </summary>
        /// <param name="fields">The fields/data representing an <see cref="AlternateNameV2"/> to parse.</param>
        /// <returns>Returns a new <see cref="AlternateNameV2"/> object.</returns>
        public override AlternateNameV2 Parse(string[] fields)
        {
            return new AlternateNameV2
            {
                Id = StringToInt(fields[0]),
                GeoNameId = StringToInt(fields[1]),
                ISOLanguage = fields[2].Length <= 3 ? fields[2] : null,
                Type = fields[2].Length <= 3 ? null : fields[2],
                Name = fields[3],
                IsPreferredName = BoolToString(fields[4]),
                IsShortName = BoolToString(fields[5]),
                IsColloquial = BoolToString(fields[6]),
                IsHistoric = BoolToString(fields[7]),
                From = fields[8],
                To = fields[9]
            };
        }

        private static bool BoolToString(string value)
        {
            return value.Equals("1");
        }
    }
}