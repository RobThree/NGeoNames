namespace NGeoNames.Entities
{
    /// <summary>
    /// Represents information about the hierarchy of an geoname.org entity.
    /// </summary>
    public class HierarchyNode
    {
        /// <summary>
        /// Gets/sets the entity's parent geoname database Id.
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// Gets/sets the entity's child geoname database Id.
        /// </summary>
        public int ChildId { get; set; }

        /// <summary>
        /// Gets/sets the type.
        /// </summary>
        /// <remarks>
        /// The type 'ADM' stands for the admin hierarchy modeled by the admin1-4 codes. The other entries are entered
        /// with the geonames.org user interface. The relation toponym-adm hierarchy is not included in the file, it
        /// can instead be built from the admincodes of the toponym.
        /// </remarks>
        public string Type { get; set; }
    }
}
