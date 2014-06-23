using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    public class TimeZoneComposer : BaseComposer<TimeZone>
    {
        public override string Compose(TimeZone value)
        {
            return string.Join(this.FieldSeparator.ToString(), value.CountryCode, value.TimeZoneId.Replace(" ", "_"), FloatToString(value.GMTOffset), 
                FloatToString(value.DSTOffset), FloatToString(value.RawOffset));
        }
    }
}