using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    public class AlternateNameComposer : BaseComposer<AlternateName>
    {
        public override string Compose(AlternateName value)
        {
            return string.Join(this.FieldSeparator.ToString(), value.Id, value.GeoNameId, string.IsNullOrEmpty(value.Type) ? value.ISOLanguage : value.Type,
                value.Name, Bool2String(value.IsPreferredName), Bool2String(value.IsShortName), Bool2String(value.IsColloquial), Bool2String(value.IsHistoric));
        }

        private static string Bool2String(bool value)
        {
            return value ? "1" : string.Empty;
        }
    }
}
