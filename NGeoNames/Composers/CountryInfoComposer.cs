using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    /// <summary>
    /// Provides methods for composing a string representing a <see cref="CountryInfo"/>.
    /// </summary>
    public class CountryInfoComposer : BaseComposer<CountryInfo>
    {
        /// <summary>
        /// Composes the specified <see cref="CountryInfo"/> into a string.
        /// </summary>
        /// <param name="value">The <see cref="CountryInfo"/> to be composed into a string.</param>
        /// <returns>A string representing the specified <see cref="CountryInfo"/>.</returns>
        public override string Compose(CountryInfo value)
        {
            return string.Join(FieldSeparator.ToString(), value.ISO_Alpha2, value.ISO_Alpha3, value.ISO_Numeric, value.FIPS, value.Country,
                value.Capital, value.Area, value.Population, value.Continent, value.Tld, value.CurrencyCode, value.CurrencyName, value.Phone,
                value.PostalCodeFormat, value.PostalCodeRegex, ArrayToValue(value.Languages), value.GeoNameId, ArrayToValue(value.Neighbours), value.EquivalentFipsCode);
        }
    }
}
