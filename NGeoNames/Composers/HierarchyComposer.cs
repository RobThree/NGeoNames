using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    /// <summary>
    /// Provides methods for composing a string representing a <see cref="HierarchyNode"/>.
    /// </summary>
    public class HierarchyComposer : BaseComposer<HierarchyNode>
    {
        /// <summary>
        /// Composes the specified <see cref="HierarchyNode"/> into a string.
        /// </summary>
        /// <param name="value">The <see cref="HierarchyNode"/> to be composed into a string.</param>
        /// <returns>A string representing the specified <see cref="HierarchyNode"/>.</returns>
        public override string Compose(HierarchyNode value)
        {
            return string.Join(FieldSeparator.ToString(), value.ParentId, value.ChildId, value.Type);
        }
    }
}
