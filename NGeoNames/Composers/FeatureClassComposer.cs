using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    public class FeatureClassComposer : BaseComposer<FeatureClass>
    {
        public override string Compose(FeatureClass value)
        {
            return string.Join(this.FieldSeparator.ToString(), value.Class, value.Description);
        }
    }
}
