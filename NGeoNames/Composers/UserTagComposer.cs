using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    public class UserTagComposer : BaseComposer<UserTag>
    {
        public override string Compose(UserTag value)
        {
            return string.Join(this.FieldSeparator.ToString(), value.GeoNameId, value.Tag);
        }
    }
}
