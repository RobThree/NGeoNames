using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    /// <summary>
    /// Provides methods for composing a string representing a <see cref="TimeZone"/>.
    /// </summary>
    public class TimeZoneComposer : BaseComposer<TimeZone>
    {
        /// <summary>
        /// Composes the specified <see cref="TimeZone"/> into a string.
        /// </summary>
        /// <param name="value">The <see cref="TimeZone"/> to be composed into a string.</param>
        /// <returns>A string representing the specified <see cref="TimeZone"/>.</returns>
        public override string Compose(TimeZone value)
        {
            return string.Join(FieldSeparator.ToString(), value.CountryCode, TimeZoneToString(value.TimeZoneId), FloatToString(value.GMTOffset), 
                FloatToString(value.DSTOffset), FloatToString(value.RawOffset));
        }
    }
}