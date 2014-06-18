using NGeoNames.Entities;

namespace NGeoNames.Parsers
{
    public class FeatureClassParser : BaseParser<FeatureClass>
    {

        private static readonly char[] fcodesep = new[] { '.' };

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
            get { return 2; }
        }


        public override FeatureClass Parse(string[] fields)
        {
            return new FeatureClass
            {
                Class = fields[0],
                Description = fields[1]
            };
        }
    }
}
