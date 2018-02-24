using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    /// <summary>
    /// Provides methods for composing a string representing a <see cref="FeatureClass"/>.
    /// </summary>
    public class FeatureClassComposer : BaseComposer<FeatureClass>
    {
        /// <summary>
        /// Composes the specified <see cref="FeatureClass"/> into a string.
        /// </summary>
        /// <param name="value">The <see cref="FeatureClass"/> to be composed into a string.</param>
        /// <returns>A string representing the specified <see cref="FeatureClass"/>.</returns>
        public override string Compose(FeatureClass value)
        {
            return string.Join(FieldSeparator.ToString(), value.Class, value.Description);
        }
    }
}
