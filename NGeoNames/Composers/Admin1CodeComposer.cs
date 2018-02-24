using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    /// <summary>
    /// Provides methods for composing a string representing an <see cref="Admin1Code"/>.
    /// </summary>
    public class Admin1CodeComposer : BaseComposer<Admin1Code>
    {
        /// <summary>
        /// Composes the specified <see cref="Admin1Code"/> into a string.
        /// </summary>
        /// <param name="value">The <see cref="Admin1Code"/> to be composed into a string.</param>
        /// <returns>A string representing the specified <see cref="Admin1Code"/>.</returns>
        public override string Compose(Admin1Code value)
        {
            return string.Join(FieldSeparator.ToString(), value.Code, value.Name, value.NameASCII, value.GeoNameId);
        }
    }
}
