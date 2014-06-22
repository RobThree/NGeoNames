using System.Text;

namespace NGeoNames.Composers
{
    public interface IComposer<T>
    {
        Encoding Encoding { get; }
        char FieldSeparator { get; }
        string Compose(T value);
    }
}
