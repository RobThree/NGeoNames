using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    /// <summary>
    /// Provides methods for composing a string representing an <see cref="Admin2Code"/>.
    /// </summary>
    public class Admin2CodeComposer : BaseComposer<Admin2Code>
    {
        /// <summary>
        /// Composes the specified <see cref="Admin2Code"/> into a string.
        /// </summary>
        /// <param name="value">The <see cref="Admin2Code"/> to be composed into a string.</param>
        /// <returns>A string representing the specified <see cref="Admin2Code"/>.</returns>
        public override string Compose(Admin2Code value)
        {
            return string.Join(FieldSeparator.ToString(), value.Code, value.Name, value.NameASCII, value.GeoNameId);
        }
    }
}
