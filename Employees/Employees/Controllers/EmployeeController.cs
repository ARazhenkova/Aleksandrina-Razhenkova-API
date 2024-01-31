using EmployeesApi.Data;
using EmployeesApi.Entities;
using EmployeesApi.Models.Common;
using EmployeesApi.Models.Employees;
using EmployeesApi.ModelToDataMappers;
using EmployeesApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EmployeesApi.Controllers
{
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEmployeeService _employeeService;
        private readonly ISecurePasswordService _securePasswordService;

        public EmployeeController(IConfiguration configuration, IEmployeeService employeeService, ISecurePasswordService securePasswordService) 
        {
            _configuration = configuration;
            _employeeService = employeeService;
            _securePasswordService = securePasswordService;
        }

        [HttpPost("/api/employees/v1")]
        [Authorize(Roles = AccessLevelsModel.ADMINISTRATOR)]
        public async Task<ActionResult<EmployeeModel>> AddAsync([FromBody] EmployeeModel employeeModel)
        {
            Employee employee = EmployeeMapper.Map(employeeModel);

            // валидиране на потребителските данни
            await _employeeService.ValidateUserDataAsync(employee, true);

            // добавяне на допълнителни данни
            employee.Uid = Guid.NewGuid().ToString().ToUpper();
            employee.Password = _securePasswordService.CreateHashWithRandomSalt(employee.Password);
            employee.Salt = _securePasswordService.GetLastGeneratedSalt();

            // валидиране на генерираните данни
            _employeeService.ValidateSensitiveInformation(employee);

            // записване в базата
            using (EmployeesDbContext dbContext = new EmployeesDbContext(_configuration))
            {
                Employee? savedEmployee = dbContext.Employee
                    .Where(e => e.Email == employee.Email)
                    .FirstOrDefault<Employee>();

                if (savedEmployee != null)
                    throw new BadHttpRequestException($"An employee with this email is already registered.");

                dbContext.Add(employee);
                dbContext.SaveChanges();
            }

            // подготвяне на изходните данни
            employeeModel.uid = Base64UrlEncoder.Encode(employee.Uid);
            employeeModel.password = string.Empty; // зачистване на паролата, защото само се подава, но никога не се връща

            return Ok(employeeModel);
        }

        [HttpGet("api/employee/{id}/v1")]
        [Authorize]
        public ActionResult<Employee> Load(string id)
        {
            string uid = Base64UrlEncoder.Decode(id);

            Employee employee = _employeeService.LoadEmployeeByUid(uid);

            EmployeeModel outputModel = EmployeeMapper.Map(employee);

            return Ok(outputModel);
        }

        [HttpPut("/api/employees/{id}/v1")]
        [Authorize(Roles = AccessLevelsModel.ADMINISTRATOR)]
        public async Task<ActionResult> UpdateAsync(string id, [FromBody] EmployeeModel employeeModel)
        {
            string uid = Base64UrlEncoder.Decode(id);
            Employee newEmployeeData = EmployeeMapper.Map(employeeModel);

            // валидиране на входните данни
            await _employeeService.ValidateUserDataAsync(newEmployeeData);

            using (EmployeesDbContext dbContext = new EmployeesDbContext(_configuration))
            {
                Employee? currentEmployeeData = dbContext.Employee
                    .Where(e => e.Uid == uid)
                    .FirstOrDefault();

                if (currentEmployeeData == null)
                    throw new BadHttpRequestException("Employee not found.");

                currentEmployeeData.Update(newEmployeeData);

                dbContext.Update(currentEmployeeData);
                dbContext.SaveChanges();
            }

            return Ok();
        }

        [HttpDelete("/api/employees/{id}/v1")]
        [Authorize(Roles = AccessLevelsModel.ADMINISTRATOR)]
        public ActionResult Delete(string id)
        {
            string uid = Base64UrlEncoder.Decode(id);

            using (EmployeesDbContext dbContext = new EmployeesDbContext(_configuration))
            {
                Employee? employee = dbContext.Employee
                    .Where(e => e.Uid == uid)
                    .FirstOrDefault();

                if (employee == null)
                    throw new BadHttpRequestException("Employee not found.");

                dbContext.Remove(employee);
                dbContext.SaveChanges();
            }

            return Ok();
        }
    }
}
