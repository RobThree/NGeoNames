using NGeoNames.Entities;
using System.Globalization;

namespace NGeoNames.Parsers
{
    /// <summary>
    /// Provides methods for parsing a <see cref="Postalcode"/> object from a string-array.
    /// </summary>
    public class PostalcodeParser : BaseParser<Postalcode>
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
            get { return 12; }
        }

        /// <summary>
        /// Parses the specified data into a <see cref="Postalcode"/> object.
        /// </summary>
        /// <param name="fields">The fields/data representing a <see cref="Postalcode"/> to parse.</param>
        /// <returns>Returns a new <see cref="Postalcode"/> object.</returns>
        public override Postalcode Parse(string[] fields)
        {
            return new Postalcode
            {
                CountryCode = fields[0],
                PostalCode = fields[1],
                PlaceName = fields[2],
                AdminName = new[] { fields[3], fields[5], fields[7] },
                AdminCode = new[] { fields[4], fields[6], fields[8] },
                Latitude = fields[9].Length > 0 ? StringToDouble(fields[9]) : double.NaN,
                Longitude = fields[10].Length > 0 ? StringToDouble(fields[10]) : double.NaN,
                Accuracy = fields[11].Length > 0 ? (int?)StringToInt(fields[11]) : null
            };
        }
    }
}
