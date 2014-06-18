
namespace NGeoNames.Entities
{
    public class TimeZone
    {
        public string CountryCode { get; set; }
        public string TimeZoneId { get; set; }
        public float GMTOffset { get; set; }
        public float DSTOffset { get; set; }
        /// <summary>
        /// Independant of DST
        /// </summary>
        public float RawOffset { get; set; }
    }
}
