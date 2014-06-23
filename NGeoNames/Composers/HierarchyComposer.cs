using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    public class HierarchyComposer : BaseComposer<HierarchyNode>
    {
        public override string Compose(HierarchyNode value)
        {
            return string.Join(this.FieldSeparator.ToString(), value.ParentId, value.ChildId, value.Type);
        }
    }
}
