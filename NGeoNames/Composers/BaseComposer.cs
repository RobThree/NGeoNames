using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace NGeoNames.Composers
{
    /// <summary>
    /// Provides an (abstract) baseclass for composers.
    /// </summary>
    /// <typeparam name="T">The type of objects to compose.</typeparam>
    public abstract class BaseComposer<T> : IComposer<T>
    {
        /// <summary>
        /// Defines the default fieldseparator (default: \t).
        /// </summary>
        public static readonly char DEFAULTFIELDSEPARATOR = '\t';

        /// <summary>
        /// Defines the default <see cref="Encoding"/> (default: UTF8).
        /// </summary>
        public static readonly Encoding DEFAULTENCODING = Encoding.UTF8;

        /// <summary>
        /// Gets the <see cref="Encoding"/> to use when writing/composing the file/stream.
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Gets the fieldseparator to use when writing/composing the file/stream.
        /// </summary>
        public char FieldSeparator { get; set; }

        /// <summary>
        /// Initializes a composer with <see cref="DEFAULTENCODING"/> and <see cref="DEFAULTFIELDSEPARATOR"/>.
        /// </summary>
        public BaseComposer()
            : this(DEFAULTENCODING, DEFAULTFIELDSEPARATOR) { }

        /// <summary>
        /// Composes the specified object into a string.
        /// </summary>
        /// <param name="value">The object to be composed into a string.</param>
        /// <returns>A string representing the specified object.</returns>
        public abstract string Compose(T value);
        
        /// <summary>
        /// Initializes a composer with the specified <see cref="Encoding"/> and fieldseparator.
        /// </summary>
        /// <param name="encoding"><see cref="Encoding"/> the composer will use.</param>
        /// <param name="fieldseparator">Field separator to use when composing the file/stream.</param>
        public BaseComposer(Encoding encoding, char fieldseparator)
        {
            Encoding = encoding;
            FieldSeparator = fieldseparator;
        }

        /// <summary>
        /// Concatenates the elements of an array, using the specified separator between each element, to form a single
        /// delimited value.
        /// </summary>
        /// <param name="values">The values to put in delimited format.</param>
        /// <param name="separator">The string to use as a separator.</param>
        /// <returns>
        /// A string that consists of the elements of values delimited by the separator string. If values is an empty
        /// array, the method returns <see cref="String.Empty"/>.
        /// </returns>
        protected string ArrayToValue(IEnumerable<string> values, string separator = ",") {
            return string.Join(separator, values);
        }

        /// <summary>
        /// Converts a float value to a string with at least 1 digit after the decimal point (e.g. 1 becomes 1.0).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A string representing the value.</returns>
        protected string FloatToString(float value)
        {
            return value.ToString("0.0#", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a double value to a string with at least 1 digit after the decimal point (e.g. 1 becomes 1.0).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A string representing the value.</returns>
        protected string DoubleToString(double value)
        {
            if (double.IsNaN(value))
                return null;
            return value.ToString("0.0#######", CultureInfo.InvariantCulture);
        }
    }
}
