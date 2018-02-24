using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    /// <summary>
    /// Provides methods for composing a string representing an <see cref="AlternateName"/>.
    /// </summary>
    public class AlternateNameComposer : BaseComposer<AlternateName>
    {
        /// <summary>
        /// Composes the specified <see cref="AlternateName"/> into a string.
        /// </summary>
        /// <param name="value">The <see cref="AlternateName"/> to be composed into a string.</param>
        /// <returns>A string representing the specified <see cref="AlternateName"/>.</returns>
        public override string Compose(AlternateName value)
        {
            return string.Join(FieldSeparator.ToString(), value.Id, value.GeoNameId, string.IsNullOrEmpty(value.Type) ? value.ISOLanguage : value.Type,
                value.Name, Bool2String(value.IsPreferredName), Bool2String(value.IsShortName), Bool2String(value.IsColloquial), Bool2String(value.IsHistoric));
        }

        private static string Bool2String(bool value)
        {
            return value ? "1" : string.Empty;
        }
    }
}
