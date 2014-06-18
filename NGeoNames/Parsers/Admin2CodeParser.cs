using NGeoNames.Entities;

namespace NGeoNames.Parsers
{
    public class Admin2CodeParser : BaseParser<Admin2Code>
    {
        public override bool HasComments
        {
            get { return false; }
        }

        public override int SkipLines
        {
            get { return 0; }
        }

        public override int ExpectedNumberOfFields
        {
            get { return 4; }
        }

        public override Admin2Code Parse(string[] fields)
        {
            return new Admin2Code
            {
                Code = fields[0],
                Name = fields[1],
                NameASCII = fields[2],
                GeoNameId = int.Parse(fields[3])
            };
        }
    }
}
