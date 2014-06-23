using NGeoNames.Entities;
using System.Globalization;

namespace NGeoNames.Composers
{
    public class ExtendedGeoNameComposer : BaseComposer<ExtendedGeoName>
    {
        public override string Compose(ExtendedGeoName value)
        {
            return string.Join(this.FieldSeparator.ToString(), value.Id, value.Name, value.ASCIIName, ArrayToValue(value.AlternateNames),
                    value.Latitude.ToString(CultureInfo.InvariantCulture), value.Longitude.ToString(CultureInfo.InvariantCulture), value.FeatureClass, value.FeatureCode, value.CountryCode,
                    ArrayToValue(value.AlternateCountryCodes), value.Admincodes[0], value.Admincodes[1], value.Admincodes[2], value.Admincodes[3],
                    value.Population, value.Elevation, value.Dem, value.Timezone.Replace(" ", "_"), value.ModificationDate.ToString("yyyy-MM-dd"));
        }
    }
}
