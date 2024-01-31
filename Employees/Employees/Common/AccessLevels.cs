namespace EmployeesApi.Settings
{
    public static class AccessLevels
    {
        public const int FISRT_ALLOWED_INDEX = 0;

        public const int EMPLOYEE           = FISRT_ALLOWED_INDEX;
        public const int ADMINISTRATOR      = FISRT_ALLOWED_INDEX + 1;

        public const int MAX_USED_INDEX = ADMINISTRATOR;
    }
}
