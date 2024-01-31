using EmployeesApi.Data;
using EmployeesApi.Entities;

namespace EmployeesApi.Services
{
    public interface IEmployeeService
    {
        Employee LoadEmployeeByUid(string uid);
        Task ValidateUserDataAsync(Employee employee, bool hasUnsecuredPassword = false);
        void ValidateSensitiveInformation(Employee employee);
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmployeeValidatorService _employeeValidatorService;

        public EmployeeService(IConfiguration configuration, IEmployeeValidatorService employeeValidatorService)
        {
            _configuration = configuration;
            _employeeValidatorService = employeeValidatorService;
        }

        public Employee LoadEmployeeByUid(string uid)
        {
            Employee? employee = null;

            using (EmployeesDbContext dbContext = new EmployeesDbContext(_configuration))
            {
                employee = dbContext.Employee
                    .Where(e => e.Uid == uid)
                    .FirstOrDefault();
            }

            if (employee == null)
                throw new BadHttpRequestException("Employee not found.");

            return employee;
        }

        /// <summary>
        /// Валидиране на потребителски данни.
        /// </summary>
        /// <param name="employee"> Подадени данни на служител. </param>
        /// <param name="hasUnsecuredPassword"> Дали има нехеширана паролата, защото тогава ще трябва да се валидира чистия и вид. </param>
        /// <exception cref="BadHttpRequestException"/>
        public async Task ValidateUserDataAsync(Employee employee, bool hasUnsecuredPassword = false)
        {
            try
            {
                _employeeValidatorService.ValidateName(employee.Name);
                _employeeValidatorService.ValidateLastName(employee.LastName);
                _employeeValidatorService.ValidateEmail(employee.Email);
                await _employeeValidatorService.ValidateCountryAlpha2CodeAsync(employee.CountryAlpha2Code);
                _employeeValidatorService.ValidateBirthDate(employee.BirthDate);
                _employeeValidatorService.ValidateDepartmentName(employee.DepartmentName);
                _employeeValidatorService.ValidatePositionName(employee.PositionName);
                _employeeValidatorService.ValidateAccessLevel(employee.AccessLevel);

                if (hasUnsecuredPassword)
                    _employeeValidatorService.ValidatePassword(employee.Password);
            }
            catch (Exception exception)
            {
                // трябва нов exception, за да може да се визуализират грешките като потребителски
                throw new BadHttpRequestException(exception.Message);
            }
        }

        public void ValidateSensitiveInformation(Employee employee)
        {
            _employeeValidatorService.ValidateUid(employee.Uid);
            _employeeValidatorService.ValidateSecurePassword(employee.Password, employee.Salt);
        }
    }
}
