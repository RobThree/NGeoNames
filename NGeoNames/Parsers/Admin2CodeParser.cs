using NGeoNames.Entities;

namespace NGeoNames.Parsers
{
    /// <summary>
    /// Provides methods for parsing an <see cref="Admin2Code"/> object from a string-array.
    /// </summary>
    public class Admin2CodeParser : BaseParser<Admin2Code>
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
            get { return 4; }
        }

        /// <summary>
        /// Parses the specified data into an <see cref="Admin2Code"/> object.
        /// </summary>
        /// <param name="fields">The fields/data representing an <see cref="Admin2Code"/> to parse.</param>
        /// <returns>Returns a new <see cref="Admin2Code"/> object.</returns>
        public override Admin2Code Parse(string[] fields)
        {
            return new Admin2Code
            {
                Code = fields[0],
                Name = fields[1],
                NameASCII = fields[2],
                GeoNameId = StringToInt(fields[3])
            };
        }
    }
}
