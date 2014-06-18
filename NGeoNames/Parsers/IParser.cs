using System.Text;
namespace NGeoNames.Parsers
{
    public interface IParser<T>
    {
        bool HasComments { get; }
        int SkipLines { get; }
        int ExpectedNumberOfFields { get; }
        Encoding Encoding { get; }
        char[] FieldSeparators { get; }
        T Parse(string[] fields);
    }
}
