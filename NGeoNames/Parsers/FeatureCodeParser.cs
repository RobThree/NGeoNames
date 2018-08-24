using NGeoNames.Entities;
using System;

namespace NGeoNames.Parsers
{
    /// <summary>
    /// Provides methods for parsing a <see cref="FeatureCode"/> object from a string-array.
    /// </summary>
    public class FeatureCodeParser : BaseParser<FeatureCode>
    {

        private static readonly char[] fcodesep = new[] { '.' };

        /// <summary>
        /// Gets wether the file/stream has (or is expected to have) comments (lines starting with "#").
        /// </summary>
        public override bool HasComments => false;

        /// <summary>
        /// Gets the number of lines to skip when parsing the file/stream (e.g. 'headers' etc.).
        /// </summary>
        public override int SkipLines => 0;

        /// <summary>
        /// Gets the number of fields the file/stream is expected to have; anything else will cause a <see cref="ParserException"/>.
        /// </summary>
        public override int ExpectedNumberOfFields => 3;

        /// <summary>
        /// Parses the specified data into a <see cref="FeatureCode"/> object.
        /// </summary>
        /// <param name="fields">The fields/data representing a <see cref="FeatureCode"/> to parse.</param>
        /// <returns>Returns a new <see cref="FeatureCode"/> object.</returns>
        public override FeatureCode Parse(string[] fields)
        {
            var d = StringToArray(fields[0], fcodesep);
            return new FeatureCode
            {
                Class = d[0].Equals("null", StringComparison.OrdinalIgnoreCase) ? null : d[0],
                Code = d.Length == 2 ? d[1] : null,
                Name = fields[1],
                Description = fields[2]
            };
        }
    }
}
