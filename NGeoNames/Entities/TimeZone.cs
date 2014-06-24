namespace NGeoNames.Entities
{
    /// <summary>
    /// Represents a timezone.
    /// </summary>
    public class TimeZone
    {
        /// <summary>
        /// Gets/sets the ISO-3166 2-letter country code of the <see cref="TimeZone"/>.
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets/sets the timezone "id" of the <see cref="TimeZone"/>.
        /// </summary>
        /// <remarks>
        /// Note that the data contains underscores ("_") for spaces but that these are replaced with actual spaces
        /// when reading data and that these spaces get replace with underscores again when writing data.
        /// </remarks>
        public string TimeZoneId { get; set; }

        /// <summary>
        /// Gets/sets the GMT ofsset (reference date Jan. 1st) of the <see cref="TimeZone"/>.
        /// </summary>
        public float GMTOffset { get; set; }

        /// <summary>
        /// Gets/sets the DST ofsset (reference date Jul. 1st) of the <see cref="TimeZone"/>.
        /// </summary>
        public float DSTOffset { get; set; }

        /// <summary>
        /// Gets/sets the raw offset (independant of DST) of the <see cref="TimeZone"/>.
        /// </summary>
        public float RawOffset { get; set; }
    }
}