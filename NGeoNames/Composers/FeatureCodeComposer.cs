using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    public class FeatureCodeComposer : BaseComposer<FeatureCode>
    {

        public override string Compose(FeatureCode value)
        {
            return string.Join(this.FieldSeparator.ToString(), GetFeatureCodeString(value), value.Name, value.Description);
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