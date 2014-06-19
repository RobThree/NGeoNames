namespace NGeoNames.Entities
{
    public class AlternateName
    {
        public int Id { get; set; }
        public int GeoNameId { get; set; }
        public string ISOLanguage { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public bool IsPreferredName { get; set; }
        public bool IsShortName { get; set; }
        public bool IsColloquial { get; set; }
        public bool IsHistoric { get; set; }
    }
}
