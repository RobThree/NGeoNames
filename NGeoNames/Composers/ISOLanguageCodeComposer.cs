using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    public class ISOLanguageCodeComposer : BaseComposer<ISOLanguageCode>
    {
        public override string Compose(ISOLanguageCode value)
        {
            return string.Join(this.FieldSeparator.ToString(), value.ISO_639_3, value.ISO_639_2, value.ISO_639_1, value.LanguageName);
        }
    }
}