using System.Text;

namespace NGeoNames.Parsers
{
    public abstract class BaseParser<T> : IParser<T>
    {
        public static readonly char[] DEFAULTSEPARATORS = { '\t' };
        public static readonly Encoding DEFAULTENCODING = Encoding.UTF8;

        public Encoding Encoding { get; set; }
        public char[] FieldSeparators { get; set; }

        public abstract bool HasComments { get; }
        public abstract int SkipLines { get; }

        public abstract int ExpectedNumberOfFields { get; }

        public abstract T Parse(string[] fields);

        public BaseParser()
            : this(DEFAULTENCODING, DEFAULTSEPARATORS) { }

        public BaseParser(Encoding encoding, char[] fieldseparators)
        {
            this.Encoding = encoding;
            this.FieldSeparators = fieldseparators;
        }
    }
}
