using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    /// <summary>
    /// Provides methods for composing a string representing a <see cref="Continent"/>.
    /// </summary>
    public class ContinentComposer : BaseComposer<Continent>
    {
        /// <summary>
        /// Composes the specified <see cref="Continent"/> into a string.
        /// </summary>
        /// <param name="value">The <see cref="Continent"/> to be composed into a string.</param>
        /// <returns>A string representing the specified <see cref="Continent"/>.</returns>
        public override string Compose(Continent value)
        {
            return string.Join(FieldSeparator.ToString(), value.Code, value.Name, value.GeoNameId);
        }
    }
}
