using System;

namespace NGeoNames.Entities
{
    /// <summary>
    /// Represents an administrative subdivision.
    /// </summary>
    public class Admin2Code
    {
        /// <summary>
        /// Gets/sets the code of the administrative subdivision.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets/sets the name of the administrative subdivision.
        /// </summary>
        public string Name { get; set; }

        private string _nameascii;
        /// <summary>
        /// Gets/sets the name of the administrative subdivision in plain ASCII.
        /// </summary>
        public string NameASCII
        {
            get { return _nameascii; }
            set
            {
                if (!value.IsASCIIOnly())
                    throw new FormatException("ASCII characters ONLY allowed");
                _nameascii = value;
            }
        }

        /// <summary>
        /// Gets/sets the geoname database Id of the administrative subdivision.
        /// </summary>
        public int GeoNameId { get; set; }
    }
}
