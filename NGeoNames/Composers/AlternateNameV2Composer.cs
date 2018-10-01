using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    /// <summary>
    /// Provides methods for composing a string representing an <see cref="AlternateNameV2"/>.
    /// </summary>
    public class AlternateNameV2Composer : BaseComposer<AlternateNameV2>
    {
        /// <summary>
        /// Composes the specified <see cref="AlternateNameV2"/> into a string.
        /// </summary>
        /// <param name="value">The <see cref="AlternateNameV2"/> to be composed into a string.</param>
        /// <returns>A string representing the specified <see cref="AlternateNameV2"/>.</returns>
        public override string Compose(AlternateNameV2 value)
        {
            return string.Join(FieldSeparator.ToString(), value.Id, value.GeoNameId, string.IsNullOrEmpty(value.Type) ? value.ISOLanguage : value.Type,
                value.Name, Bool2String(value.IsPreferredName), Bool2String(value.IsShortName), Bool2String(value.IsColloquial), Bool2String(value.IsHistoric),
                value.From, value.To);
        }

        private static string Bool2String(bool value)
        {
            return value ? "1" : string.Empty;
        }
    }
}
