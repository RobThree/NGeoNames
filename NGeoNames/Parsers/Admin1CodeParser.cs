using NGeoNames.Entities;

namespace NGeoNames.Parsers
{
    public class Admin1CodeParser : BaseParser<Admin1Code>
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


        public override Admin1Code Parse(string[] fields)
        {
            return new Admin1Code
            {
                Code = fields[0],
                Name = fields[1],
                NameASCII = fields[2],
                GeoNameId = int.Parse(fields[3])
            };
        }
    }
}
