using NGeoNames.Composers;
using NGeoNames.Entities;
using NGeoNamesTests;
using System.Text;

namespace NGeoNames
{
    internal class CustomComposer : BaseComposer<CustomEntity>
    {
        public CustomComposer(Encoding encoding, char fieldseparator)
        {
            this.Encoding = encoding;
            this.FieldSeparator = fieldseparator;
        }

        public override string Compose(CustomEntity value)
        {
            return string.Join(this.FieldSeparator.ToString(), value.Data);
        }
    }
}
