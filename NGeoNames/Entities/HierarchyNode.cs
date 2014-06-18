namespace NGeoNames.Entities
{
    public class HierarchyNode
    {
        public int ParentId { get; set; }
        public int ChildId { get; set; }
        public string Type { get; set; }
    }
}
