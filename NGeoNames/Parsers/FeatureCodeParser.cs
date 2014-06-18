using NGeoNames.Entities;
using System;

namespace NGeoNames.Parsers
{
    public class FeatureCodeParser : BaseParser<FeatureCode>
    {

        private static readonly char[] fcodesep = new[] { '.' };

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
            get { return 3; }
        }


        public override FeatureCode Parse(string[] fields)
        {
            var d = fields[0].Split(fcodesep);
            return new FeatureCode
            {
                Class = d[0].Equals("null", StringComparison.OrdinalIgnoreCase) ? null : d[0],
                Code = d.Length == 2 ? d[1] : null,
                Name = fields[1],
                Description = fields[2]
            };
        }
    }
}
