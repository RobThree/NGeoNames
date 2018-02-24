using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    /// <summary>
    /// Provides methods for composing a string representing a <see cref="GeoName"/>.
    /// </summary>
    public class GeoNameComposer : BaseComposer<GeoName>
    {
        /// <summary>
        /// Gets wether the instance of the <see cref="GeoNameComposer"/> uses extended fileformat (19 fields) or
        /// "compact format" (4 fields: Id, Name, Latitude and Longitude).
        /// </summary>
        public bool UseExtendedFileFormat { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoNameComposer"/> class with default values (extended fileformat, 19 fields).
        /// </summary>
        public GeoNameComposer()
            : this(true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoNameComposer"/> class with specified file format.
        /// </summary>
        /// <param name="useextendedfileformat">
        /// When this parameter is true, the (default) file format (19 fields) will be assumed for geoname data,
        /// when this parameter is false, the "compact file format" (4 fields: Id, Name, Latitude and Longitude)
        /// will be assumed.
        /// </param>
        public GeoNameComposer(bool useextendedfileformat)
        {
            UseExtendedFileFormat = useextendedfileformat;
        }

        /// <summary>
        /// Composes the specified <see cref="GeoName"/> into a string.
        /// </summary>
        /// <param name="value">The <see cref="GeoName"/> to be composed into a string.</param>
        /// <returns>A string representing the specified <see cref="GeoName"/>.</returns>
        public override string Compose(GeoName value)
        {
            if (UseExtendedFileFormat)
            {
                return string.Join(FieldSeparator.ToString(), value.Id, value.Name, null, null, DoubleToString(value.Latitude), DoubleToString(value.Longitude),
                    null, null, null, null, null, null, null, null, null, null, null, null, null);
            }
            else
            {
                return string.Join(FieldSeparator.ToString(), value.Id, value.Name, DoubleToString(value.Latitude), DoubleToString(value.Longitude));
            }
        }
    }
}
