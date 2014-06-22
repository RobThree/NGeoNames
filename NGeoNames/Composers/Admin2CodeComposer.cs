using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    public class Admin2CodeComposer : BaseComposer<Admin2Code>
    {
        public override string Compose(Admin2Code value)
        {
            return string.Join(this.FieldSeparator.ToString(), value.Code, value.Name, value.NameASCII, value.GeoNameId);
        }
    }
}
