using System;
using System.Globalization;
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
        /// Internal constant for String->String[] conversion
        /// </summary>
        private static readonly char[] csv = { ',' };

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
            Encoding = encoding;
            FieldSeparators = fieldseparators;
        }

        /// <summary>
        /// Converts a string into an integer.
        /// </summary>
        /// <param name="value">A string containing the number to convert.</param>
        /// <returns>An integer equivalent to the number contained in <paramref name="value"/>.</returns>
        protected int StringToInt(string value)
        {
            return int.Parse(value);
        }

        /// <summary>
        /// Converts a string into a long.
        /// </summary>
        /// <param name="value">A string containing the number to convert.</param>
        /// <returns>A long equivalent to the number contained in <paramref name="value"/>.</returns>
        protected long StringToLong(string value)
        {
            return long.Parse(value);
        }

        /// <summary>
        /// Converts a comma-separated string into an array of strings.
        /// </summary>
        /// <param name="value">A string containing the values separated by a comma.</param>
        /// <returns>A string array equivalent to the values contained in <paramref name="value"/>.</returns>
        protected string[] StringToArray(string value)
        {
            return StringToArray(value, csv);
        }

        /// <summary>
        /// Converts a delimited string into an array of strings.
        /// </summary>
        /// <param name="value">A string containing the values separated by a delimiter.</param>
        /// <param name="delimiter">The delimiter(s) of the string.</param>
        /// <returns>A string array equivalent to the values contained in <paramref name="value"/>.</returns>
        protected string[] StringToArray(string value, char[] delimiter)
        {
            return value.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Converts a string into a float.
        /// </summary>
        /// <param name="value">A string containing the number to convert.</param>
        /// <returns>A float equivalent to the number contained in <paramref name="value"/>.</returns>
        protected float StringToFloat(string value)
        {
            return float.Parse(value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a string into a double.
        /// </summary>
        /// <param name="value">A string containing the number to convert.</param>
        /// <returns>A double equivalent to the number contained in <paramref name="value"/>.</returns>
        protected double StringToDouble(string value)
        {
            return double.Parse(value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a string into a datetime.
        /// </summary>
        /// <param name="value">A string containing the datetime to convert.</param>
        /// <param name="format">The format </param>
        /// <returns>A DateTime equivalent to the datetime contained in <paramref name="value"/>.</returns>
        protected DateTime StringToDateTime(string value, string format = "yyyy-MM-dd")
        {
            return DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a string to a timezone from geoname files (where a space is represented by an underscore).
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A string representing the value.</returns>
        protected string StringToTimeZone(string value)
        {
            return value.Replace("_", " ");
        }
    }
}