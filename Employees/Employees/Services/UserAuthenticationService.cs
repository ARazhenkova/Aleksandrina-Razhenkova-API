using EmployeesApi.Data;
using EmployeesApi.Entities;

namespace EmployeesApi.Services
{
    public interface IUserAuthenticationService
    {
        Employee? FindEmployeeByEmail(string email);
        bool IsEnteredPasswordCorrect(Employee employee, string enteredPassword);
        string CreateToken(Employee employee);
    }

    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly IJwtService _jwtService;
        private readonly ISecurePasswordService _securePasswordService;

        public UserAuthenticationService(IConfiguration configuration, IJwtService jwtService, ISecurePasswordService securePasswordService) 
        {
            _configuration = configuration;
            _jwtService = jwtService;
            _securePasswordService = securePasswordService;
        }

        public Employee? FindEmployeeByEmail(string email)
        {
            Employee? employee = null;

            using (EmployeesDbContext dbContext = new EmployeesDbContext(_configuration))
            {
                employee = dbContext.Employee
                    .Where(e => e.Email == email)
                    .FirstOrDefault<Employee>();
            }

            return employee;
        }

        public bool IsEnteredPasswordCorrect(Employee employee, string rawUserPassword)
        {
            string userPassword = _securePasswordService.RecreateHash(rawUserPassword, employee.Salt);

            if (employee.Password != userPassword)
                return false;

            return true;
        }

        public string CreateToken(Employee employee)
        {
            return _jwtService.CreateToken(employee);
        }
    }
}
