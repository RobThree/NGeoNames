using System.Text;

namespace NGeoNames.Composers
{
    /// <summary>
    /// Defines a generalized composer to compose geoname.org files of a specific type.
    /// </summary>
    /// <typeparam name="T">The type of objects to compose.</typeparam>
    public interface IComposer<T>
    {
        /// <summary>
        /// Gets the <see cref="Encoding"/> to use when writing/composing the file/stream.
        /// </summary>
        Encoding Encoding { get; }

        /// <summary>
        /// Gets the field separator to use when writing/composing the file/stream. 
        /// </summary>
        char FieldSeparator { get; }

        /// <summary>
        /// Composes a string representing an object of type T to be used in geoname.org files.
        /// </summary>
        /// <param name="value">An object that represents the value to be written.</param>
        /// <returns>A string representing an object of type T to be used in geoname.org files.</returns>
        string Compose(T value);
    }
}
