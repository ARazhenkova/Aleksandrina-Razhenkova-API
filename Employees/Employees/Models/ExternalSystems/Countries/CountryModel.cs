namespace EmployeesApi.Models.ExternalSystems.Countries
{
    public class CountryModel
    {
        public required string name { get; set; }
        public required string capital { get; set; }
        public required string iso2 { get; set; }
        public required string iso3 { get; set; }
    }
}
