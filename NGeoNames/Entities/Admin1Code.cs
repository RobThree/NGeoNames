using System;

namespace NGeoNames.Entities
{
    /// <summary>
    /// Represents an administrative division.
    /// </summary>
    public class Admin1Code
    {
        /// <summary>
        /// Gets/sets the code of the administrative division.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets/sets the name of the administrative division.
        /// </summary>
        public string Name { get; set; }

        private string _nameascii;
        /// <summary>
        /// Gets/sets the name of the administrative division in plain ASCII.
        /// </summary>
        public string NameASCII {
            get { return _nameascii; }
            set
            {
                if (!value.IsASCIIOnly())
                    throw new FormatException("ASCII characters ONLY allowed");
                _nameascii = value;
            }
        }

        /// <summary>
        /// Gets/sets the geoname database Id of the administrative division.
        /// </summary>
        public int GeoNameId { get; set; }
    }
}
