using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    /// <summary>
    /// Provides methods for composing a string representing an <see cref="ISOLanguageCode"/>.
    /// </summary>
    public class ISOLanguageCodeComposer : BaseComposer<ISOLanguageCode>
    {
        /// <summary>
        /// Composes the specified <see cref="ISOLanguageCode"/> into a string.
        /// </summary>
        /// <param name="value">The <see cref="ISOLanguageCode"/> to be composed into a string.</param>
        /// <returns>A string representing the specified <see cref="ISOLanguageCode"/>.</returns>
        public override string Compose(ISOLanguageCode value)
        {
            return string.Join(FieldSeparator.ToString(), value.ISO_639_3, value.ISO_639_2, value.ISO_639_1, value.LanguageName);
        }
    }
}