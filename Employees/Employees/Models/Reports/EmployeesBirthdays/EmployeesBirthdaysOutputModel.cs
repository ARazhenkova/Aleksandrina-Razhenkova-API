using EmployeesApi.Models.Employees;

namespace EmployeesApi.Models.Reports.EmployeesBirthdays
{
    public class EmployeesBirthdaysOutputModel
    {
        public List<EmployeeModel> Employees { get; set; } = new List<EmployeeModel>();
    }
}
