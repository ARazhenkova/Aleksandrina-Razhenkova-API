using EmployeesApi.Models.Common;
using EmployeesApi.Settings;

namespace EmployeesApi.ModelToDataMappers
{
    public class AccessLevelMapper
    {
        public static int Map(string accessLevel)
        {
            if (accessLevel == AccessLevelsModel.EMPLOYEE)
                return AccessLevels.EMPLOYEE;
            else if (accessLevel == AccessLevelsModel.ADMINISTRATOR)
                return AccessLevels.ADMINISTRATOR;
            else
                throw new BadHttpRequestException("Invalid access level.");
        }

        public static string Map(int accessLevel)
        {
            if (accessLevel == AccessLevels.EMPLOYEE)
                return AccessLevelsModel.EMPLOYEE;
            else if (accessLevel == AccessLevels.ADMINISTRATOR)
                return AccessLevelsModel.ADMINISTRATOR;
            else
                throw new ArgumentException("Invalid access level.");
        }
    }
}
