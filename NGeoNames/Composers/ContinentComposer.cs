using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    public class ContinentComposer : BaseComposer<Continent>
    {
        public override string Compose(Continent value)
        {
            return string.Join(this.FieldSeparator.ToString(), value.Code, value.Name, value.Id);
        }
    }
}
