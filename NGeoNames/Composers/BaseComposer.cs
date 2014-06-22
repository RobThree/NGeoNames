using System.Collections.Generic;
using System.Text;

namespace NGeoNames.Composers
{
    public abstract class BaseComposer<T> : IComposer<T>
    {
        public static readonly char DEFAULTSEPARATOR = '\t';
        public static readonly Encoding DEFAULTENCODING = Encoding.UTF8;

        public Encoding Encoding { get; set; }
        public char FieldSeparator { get; set; }

        public BaseComposer()
            : this(DEFAULTENCODING, DEFAULTSEPARATOR) { }

        public BaseComposer(Encoding encoding, char fieldseparator)
        {
            this.Encoding = encoding;
            this.FieldSeparator = fieldseparator;
        }

        public abstract string Compose(T value);

        protected string ArrayToValue(IEnumerable<string> values, string separator = ",") {
            return string.Join(separator, values);
        }
    }
}
