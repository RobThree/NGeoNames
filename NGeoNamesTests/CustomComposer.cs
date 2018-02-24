using NGeoNames.Composers;
using NGeoNamesTests;
using System.Text;

namespace NGeoNames
{
    internal class CustomComposer : BaseComposer<CustomEntity>
    {
        public CustomComposer(Encoding encoding, char fieldseparator)
        {
            Encoding = encoding;
            FieldSeparator = fieldseparator;
        }

        public override string Compose(CustomEntity value)
        {
            return string.Join(FieldSeparator.ToString(), value.Data);
        }
    }
}
