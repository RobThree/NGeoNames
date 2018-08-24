using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    /// <summary>
    /// Provides methods for composing a string representing a <see cref="Postalcode"/>.
    /// </summary>
    public class PostalcodeComposer : BaseComposer<Postalcode>
    {
        /// <summary>
        /// Composes the specified <see cref="Postalcode"/> into a string.
        /// </summary>
        /// <param name="value">The <see cref="Postalcode"/> to be composed into a string.</param>
        /// <returns>A string representing the specified <see cref="Postalcode"/>.</returns>
        public override string Compose(Postalcode value)
        {
            return string.Join(FieldSeparator.ToString(), value.CountryCode, value.PostalCode, value.PlaceName,
                GetArrayValue(value.AdminName, 0), GetArrayValue(value.AdminCode, 0),
                GetArrayValue(value.AdminName, 1), GetArrayValue(value.AdminCode, 1),
                GetArrayValue(value.AdminName, 2), GetArrayValue(value.AdminCode, 2),
                DoubleToString(value.Latitude), DoubleToString(value.Longitude), value.Accuracy
            );
        }
    }
}
