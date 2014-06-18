using NGeoNames.Entities;

namespace NGeoNames.Parsers
{
    public class ContinentParser : BaseParser<Continent>
    {
        public override bool HasComments
        {
            get { return true; }
        }

        public override int SkipLines
        {
            get { return 0; }
        }

        public override int ExpectedNumberOfFields
        {
            get { return 3; }
        }

        public override Continent Parse(string[] fields)
        {
            return new Continent
            {
                Code = fields[0],
                Name = fields[1],
                Id = int.Parse(fields[2])
            };
        }
    }
}
