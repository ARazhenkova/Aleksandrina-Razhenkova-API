using EmployeesApi.Data;
using EmployeesApi.Models.ExternalSystems.Countries;

namespace EmployeesApi.ModelToDataMappers
{
    public static class CountryMapper
    {
        public static Country Map(CountryModel countryModel)
        {
            Country country = new Country();

            country.Name            = countryModel.name;
            country.CapitalName     = countryModel.capital;
            country.AlphaCode2      = countryModel.iso2;
            country.AlphaCode3      = countryModel.iso3;

            return country;
        }
    }
}
