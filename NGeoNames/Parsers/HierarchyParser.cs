using NGeoNames.Entities;

namespace NGeoNames.Parsers
{
    public class HierarchyParser : BaseParser<HierarchyNode>
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
            get { return 3; }
        }

        public override HierarchyNode Parse(string[] fields)
        {
            return new HierarchyNode
            {
                ParentId = int.Parse(fields[0]),
                ChildId = int.Parse(fields[1]),
                Type = fields[2]
            };
        }
    }
}
