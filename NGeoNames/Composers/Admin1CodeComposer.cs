using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    public class Admin1CodeComposer : BaseComposer<Admin1Code>
    {
        public override string Compose(Admin1Code value)
        {
            return string.Join(this.FieldSeparator.ToString(), value.Code, value.Name, value.NameASCII, value.GeoNameId);
        }
    }
}
