using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    /// <summary>
    /// Provides methods for composing a string representing a <see cref="FeatureCode"/>.
    /// </summary>
    public class FeatureCodeComposer : BaseComposer<FeatureCode>
    {
        /// <summary>
        /// Composes the specified <see cref="FeatureCode"/> into a string.
        /// </summary>
        /// <param name="value">The <see cref="FeatureCode"/> to be composed into a string.</param>
        /// <returns>A string representing the specified <see cref="FeatureCode"/>.</returns>
        public override string Compose(FeatureCode value)
        {
            return string.Join(FieldSeparator.ToString(), GetFeatureCodeString(value), value.Name, value.Description);
        }

        private string GetFeatureCodeString(FeatureCode value)
        {
            if (!string.IsNullOrEmpty(value.Class) && (!string.IsNullOrEmpty(value.Code)))
                return string.Format("{0}.{1}", value.Class, value.Code);
            if (!string.IsNullOrEmpty(value.Class))
                return value.Class;
            if (!string.IsNullOrEmpty(value.Code))
                return value.Code;
            return "null";
        }
    }
}