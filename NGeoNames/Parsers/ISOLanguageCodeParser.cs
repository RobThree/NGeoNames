using NGeoNames.Entities;
namespace NGeoNames.Parsers
{
    /// <summary>
    /// Provides methods for parsing an <see cref="ISOLanguageCode"/> object from a string-array.
    /// </summary>
    public class ISOLanguageCodeParser : BaseParser<ISOLanguageCode>
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
            get { return 4; }
        }

        /// <summary>
        /// Parses the specified data into an <see cref="ISOLanguageCode"/> object.
        /// </summary>
        /// <param name="fields">The fields/data representing an <see cref="ISOLanguageCode"/> to parse.</param>
        /// <returns>Returns a new <see cref="ISOLanguageCode"/> object.</returns>
        public override ISOLanguageCode Parse(string[] fields)
        {
            return new ISOLanguageCode
            {
                ISO_639_3 = fields[0],
                ISO_639_2 = fields[1],
                ISO_639_1 = fields[2],
                LanguageName = fields[3]
            };
        }
    }
}
