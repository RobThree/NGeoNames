using System.Text;
namespace NGeoNames.Parsers
{
    /// <summary>
    /// Defines a generalized parser to parse geoname.org files of a specific type.
    /// </summary>
    /// <typeparam name="T">The type of objects to parse.</typeparam>
    public interface IParser<T>
    {
        /// <summary>
        /// Gets wether the file/stream has (or is expected to have) comments (lines starting with "#").
        /// </summary>
        bool HasComments { get; }

        /// <summary>
        /// Gets the number of lines to skip when parsing the file/stream (e.g. 'headers' etc.).
        /// </summary>
        int SkipLines { get; }

        /// <summary>
        /// Gets the number of fields the file/stream is expected to have; anything else will cause a <see cref="ParserException"/>.
        /// </summary>
        int ExpectedNumberOfFields { get; }

        /// <summary>
        /// Gets the <see cref="Encoding"/> to use when reading/parsing the file/stream.
        /// </summary>
        Encoding Encoding { get; }

        /// <summary>
        /// Gets an array of chars that define one or more fieldseparators.
        /// </summary>
        char[] FieldSeparators { get; }

        /// <summary>
        /// Parses the specified fields into an object of type T.
        /// </summary>
        /// <param name="fields">The fields to be parsed.</param>
        /// <returns>An object of type T parsed from the file/stream.</returns>
        T Parse(string[] fields);
    }
}
