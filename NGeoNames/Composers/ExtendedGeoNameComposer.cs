using NGeoNames.Entities;
using System.Globalization;

namespace NGeoNames.Composers
{
    /// <summary>
    /// Provides methods for composing a string representing an <see cref="ExtendedGeoName"/>.
    /// </summary>
    public class ExtendedGeoNameComposer : BaseComposer<ExtendedGeoName>
    {
        /// <summary>
        /// Composes the specified <see cref="ExtendedGeoName"/> into a string.
        /// </summary>
        /// <param name="value">The <see cref="ExtendedGeoName"/> to be composed into a string.</param>
        /// <returns>A string representing the specified <see cref="ExtendedGeoName"/>.</returns>
        public override string Compose(ExtendedGeoName value)
        {
            return string.Join(FieldSeparator.ToString(), value.Id, value.Name, value.NameASCII, ArrayToValue(value.AlternateNames),
                    value.Latitude.ToString(CultureInfo.InvariantCulture), value.Longitude.ToString(CultureInfo.InvariantCulture), value.FeatureClass, value.FeatureCode, value.CountryCode,
                    ArrayToValue(value.AlternateCountryCodes), GetArrayValue(value.Admincodes, 0), GetArrayValue(value.Admincodes, 1), GetArrayValue(value.Admincodes, 2), GetArrayValue(value.Admincodes, 3),
                    value.Population, value.Elevation, value.Dem, TimeZoneToString(value.Timezone), DateTimeToString(value.ModificationDate));
        }
    }
}
