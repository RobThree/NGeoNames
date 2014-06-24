using System.Text;

namespace NGeoNames.Parsers
{
    /// <summary>
    /// Provides an (abstract) baseclass for parsers.
    /// </summary>
    /// <typeparam name="T">The type of objects to parse.</typeparam>
    public abstract class BaseParser<T> : IParser<T>
    {
        /// <summary>
        /// Defines the default fieldseparator(s) (default: \t).
        /// </summary>
        public static readonly char[] DEFAULTFIELDSEPARATORS = { '\t' };

        /// <summary>
        /// Defines the default <see cref="Encoding"/> (default: UTF8).
        /// </summary>
        public static readonly Encoding DEFAULTENCODING = Encoding.UTF8;

        /// <summary>
        /// Gets the <see cref="Encoding"/> to use when reading/parsing the file/stream.
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Gets an array of chars that define one or more fieldseparators.
        /// </summary>
        public char[] FieldSeparators { get; set; }

        /// <summary>
        /// Gets wether the file/stream has (or is expected to have) comments (lines starting with "#").
        /// </summary>
        public abstract bool HasComments { get; }

        /// <summary>
        /// Gets the number of lines to skip when parsing the file/stream (e.g. 'headers' etc.).
        /// </summary>
        public abstract int SkipLines { get; }

        /// <summary>
        /// Gets the number of fields the file/stream is expected to have; anything else will cause a <see cref="ParserException"/>.
        /// </summary>
        public abstract int ExpectedNumberOfFields { get; }

        /// <summary>
        /// Parses the specified fields into an object of type T.
        /// </summary>
        /// <param name="fields">The fields to be parsed.</param>
        /// <returns>An object of type T parsed from the file/stream.</returns>
        public abstract T Parse(string[] fields);

        /// <summary>
        /// Initializes a parser with <see cref="DEFAULTENCODING"/> and <see cref="DEFAULTFIELDSEPARATORS"/>.
        /// </summary>
        public BaseParser()
            : this(DEFAULTENCODING, DEFAULTFIELDSEPARATORS) { }

        /// <summary>
        /// Initializes a parser with the specified <see cref="Encoding"/> and fieldseparator(s).
        /// </summary>
        /// <param name="encoding"><see cref="Encoding"/> the parser will use.</param>
        /// <param name="fieldseparators">Field separator(s) to use when parsing the file/stream.</param>
        public BaseParser(Encoding encoding, char[] fieldseparators)
        {
            this.Encoding = encoding;
            this.FieldSeparators = fieldseparators;
        }
    }
}
