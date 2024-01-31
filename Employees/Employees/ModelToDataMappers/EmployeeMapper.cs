using EmployeesApi.Data;
using EmployeesApi.Models.Employees;
using Microsoft.IdentityModel.Tokens;

namespace EmployeesApi.ModelToDataMappers
{
    public static class EmployeeMapper
    {
        public static Employee Map(EmployeeModel employeeModel)
        {
            Employee employee = new Employee();

            if (string.IsNullOrWhiteSpace(employeeModel.uid) == false)
                employee.Uid = Base64UrlEncoder.Decode(employeeModel.uid);

            employee.Name               = employeeModel.name;
            employee.LastName           = employeeModel.lastName;
            employee.Email              = employeeModel.email;
            employee.CountryAlpha2Code  = employeeModel.countryAlpha2Code;
            employee.BirthDate          = employeeModel.birthDate;
            employee.DepartmentName     = employeeModel.departmentName;
            employee.PositionName       = employeeModel.positionName;
            employee.AccessLevel        = AccessLevelMapper.Map(employeeModel.accessLevel);

            // подава се само при регистриране на нов служител
            if (string.IsNullOrWhiteSpace(employeeModel.password) == false)
                employee.Password = employeeModel.password; 

            return employee;
        }

        public static EmployeeModel Map(Employee employee)
        {
            EmployeeModel employeeModel = new EmployeeModel();

            if (string.IsNullOrWhiteSpace(employee.Uid) == false)
                employeeModel.uid = Base64UrlEncoder.Encode(employee.Uid);

            employeeModel.name              = employee.Name;
            employeeModel.lastName          = employee.LastName;
            employeeModel.email             = employee.Email;
            employeeModel.countryAlpha2Code = employee.CountryAlpha2Code;
            employeeModel.birthDate         = employee.BirthDate;
            employeeModel.departmentName    = employee.DepartmentName;
            employeeModel.positionName      = employee.PositionName;
            employeeModel.accessLevel       = AccessLevelMapper.Map(employee.AccessLevel);

            return employeeModel;
        }
    }
}
