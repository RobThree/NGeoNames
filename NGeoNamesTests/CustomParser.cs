using Microsoft.VisualStudio.TestTools.UnitTesting;
using NGeoNames.Parsers;
using System.Text;

namespace NGeoNamesTests
{
    internal class CustomParser : IParser<CustomEntity>
    {
        public bool HasComments { get; private set; }
        public int SkipLines { get; private set; }
        public int ExpectedNumberOfFields { get; private set; }
        public Encoding Encoding { get; private set; }
        public char[] FieldSeparators { get; private set; }

        public CustomParser(int expectedfields, int skiplines, char[] fieldseparators, Encoding encoding, bool hascomments)
        {
            SkipLines = skiplines;
            ExpectedNumberOfFields = expectedfields;
            FieldSeparators = fieldseparators;
            Encoding = encoding;
            HasComments = hascomments;
        }

        public CustomEntity Parse(string[] fields)
        {
            Assert.AreEqual(ExpectedNumberOfFields, fields.Length);
            return new CustomEntity { Data = fields };
        }
    }
}
