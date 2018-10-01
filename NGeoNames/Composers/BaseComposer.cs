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
        /// array, the method returns <see cref="string.Empty"/>.
        /// </returns>
        protected string ArrayToValue(IEnumerable<string> values, string separator = ",") {
            if (values == null)
                return null;
            return string.Join(separator, values);
        }

        /// <summary>
        /// Returns the desired element of a string array, or, when the index is out of bounds, <see langword="null"/>.
        /// </summary>
        /// <param name="values">The array to return the element from.</param>
        /// <param name="index">The index of the element to get.</param>
        /// <returns>
        /// Returns the desired element of a string array or, when the index is out of bounds, <see langword="null"/>.
        /// </returns>
        protected string GetArrayValue(string[] values, int index)
        {
            return GetArrayValue(values, index, null);
        }

        /// <summary>
        /// Returns the desired element of an array, or, when the index is out of bounds, the specified
        /// <paramref name="defaultValue"/>.
        /// </summary>
        /// <typeparam name="TVal">The type of the value returned.</typeparam>
        /// <param name="values">The array to return the element from.</param>
        /// <param name="index">The index of the element to get.</param>
        /// <param name="defaultValue">The default value to return when the index is out of bounds.</param>
        /// <returns>
        /// Returns the desired element of a string array or, when the index is out of bounds, the specified
        /// <paramref name="defaultValue"/>.
        /// </returns>
        protected TVal GetArrayValue<TVal>(TVal[] values, int index, TVal defaultValue)
        {
            if (values != null && index < values.Length)
                return values[index];
            return defaultValue;
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

        /// <summary>
        /// Converts a timezone string to a timezonestring for geoname files (where a space is converted to an underscore).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A string representing the value.</returns>
        protected string TimeZoneToString(string value)
        {
            return value?.Replace(" ", "_");
        }

        /// <summary>
        /// Converts a datetime to a string using the specified format.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="format">The format to use.</param>
        /// <returns>A string representing the value.</returns>
        protected string DateTimeToString(DateTime value, string format = "yyyy-MM-dd")
        {
            return value.ToString(format, CultureInfo.InvariantCulture);
        }
    }
}
