namespace EmployeesApi.Models.ExternalSystems.Countries
{
    public class LoadCountriesOutputModel
    {
        public required bool error { get; set; }
        public required string msg { get; set; }
        public required List<CountryModel> data { get; set; }
    }
}
