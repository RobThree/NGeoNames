namespace NGeoNames.Entities
{
    public class GeoName
    {
        /// <summary>
        /// Integer id of record in geonames database
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of geographical point (utf8) varchar(200)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Latitude in decimal degrees (wgs84)
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// Longitude in decimal degrees (wgs84)
        /// </summary>
        public double Longitude { get; set; }

        public double DistanceTo(GeoName loc)
        {
            return GeoUtil.DistanceTo(this, loc);
        }
    }
}
