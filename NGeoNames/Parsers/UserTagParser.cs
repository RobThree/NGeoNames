using NGeoNames.Entities;

namespace NGeoNames.Parsers
{
    public class UserTagParser : BaseParser<UserTag>
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
            get { return 2; }
        }

        public override UserTag Parse(string[] fields)
        {
            return new UserTag
            {
                GeoNameId = int.Parse(fields[0]),
                Tag = fields[1]
            };
        }
    }
}
