using NGeoNames.Entities;

namespace NGeoNames.Parsers
{
    /// <summary>
    /// Provides methods for parsing a <see cref="HierarchyNode"/> object from a string-array.
    /// </summary>
    public class HierarchyParser : BaseParser<HierarchyNode>
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
            get { return 3; }
        }

        /// <summary>
        /// Parses the specified data into a <see cref="HierarchyNode"/> object.
        /// </summary>
        /// <param name="fields">The fields/data representing a <see cref="HierarchyNode"/> to parse.</param>
        /// <returns>Returns a new <see cref="HierarchyNode"/> object.</returns>
        public override HierarchyNode Parse(string[] fields)
        {
            return new HierarchyNode
            {
                ParentId = StringToInt(fields[0]),
                ChildId = StringToInt(fields[1]),
                Type = fields[2]
            };
        }
    }
}
