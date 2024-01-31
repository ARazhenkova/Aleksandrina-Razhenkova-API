namespace EmployeesApi.Settings
{
    public static class AppConfigurations
    {
        public static class ConnectionStrings
        {
            public const string EMPLOYEES = "EmployeesDbConnectionString";
        }

        public static class JwtSettings
        {
            public const string SYMMETRIC_SECURITY_KEY = "SymmetricSecurityKey";
            public const string TOKEN_LIFE = "TokenLife";

            public static string GetSectionName() => "JwtSettings";
        }

        public const string LOADING_COUNTRIES_URL = "LoadingCountriesUrl";
        public const string LEGAL_WORKING_AGE = "LegalWorkingAge";

        public static string GetFileName() => "appsettings.json";
    }
}
