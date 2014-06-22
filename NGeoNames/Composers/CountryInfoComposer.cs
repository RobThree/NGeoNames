using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    public class CountryInfoComposer : BaseComposer<CountryInfo>
    {
        public override string Compose(CountryInfo value)
        {
            return string.Join(this.FieldSeparator.ToString(), value.ISO_Alpha2, value.ISO_Alpha3, value.ISO_Numeric, value.FIPS, value.Country,
                value.Capital, value.Area, value.Population, value.Continent, value.Tld, value.CurrencyCode, value.CurrencyName, value.Phone,
                value.PostalCodeFormat, value.PostalCodeRegex, ArrayToValue(value.Languages), value.GeoNameId, ArrayToValue(value.Neighbours), value.EquivalentFipsCode);
        }
    }
}
