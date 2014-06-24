using NGeoNames.Entities;

namespace NGeoNames.Parsers
{
    /// <summary>
    /// Provides methods for parsing a <see cref="FeatureClass"/> object from a string-array.
    /// </summary>
    public class FeatureClassParser : BaseParser<FeatureClass>
    {

        private static readonly char[] fcodesep = new[] { '.' };

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
            get { return 2; }
        }

        /// <summary>
        /// Parses the specified data into a <see cref="FeatureClass"/> object.
        /// </summary>
        /// <param name="fields">The fields/data representing a <see cref="FeatureClass"/> to parse.</param>
        /// <returns>Returns a new <see cref="FeatureClass"/> object.</returns>
        public override FeatureClass Parse(string[] fields)
        {
            return new FeatureClass
            {
                Class = fields[0],
                Description = fields[1]
            };
        }
    }
}
