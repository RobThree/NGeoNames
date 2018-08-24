using NGeoNames.Entities;

namespace NGeoNames.Parsers
{
    /// <summary>
    /// Provides methods for parsing a <see cref="ContinentParser"/> object from a string-array.
    /// </summary>
    public class ContinentParser : BaseParser<Continent>
    {
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
            get { return 3; }
        }

        /// <summary>
        /// Parses the specified data into a <see cref="Continent"/> object.
        /// </summary>
        /// <param name="fields">The fields/data representing a <see cref="Continent"/> to parse.</param>
        /// <returns>Returns a new <see cref="Continent"/> object.</returns>
        public override Continent Parse(string[] fields)
        {
            return new Continent
            {
                Code = fields[0],
                Name = fields[1],
                GeoNameId = StringToInt(fields[2])
            };
        }
    }
}
