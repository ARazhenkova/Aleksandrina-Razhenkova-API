using EmployeesApi.Data;
using EmployeesApi.Entities;
using EmployeesApi.Models.Employees;
using EmployeesApi.Models.Reports.EmployeesBirthdays;
using EmployeesApi.ModelToDataMappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesApi.Controllers.Reports
{
    [ApiController]
    public class EmployeesReportsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmployeesReportsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("/api/reports/employees/birthdays/v1")]
        [Authorize]
        public ActionResult<EmployeesBirthdaysOutputModel> Birthdays([FromBody] EmployeesBirthdaysInputModel inputModel)
        {
            List<Employee> employees = new List<Employee>();
            DateTime currentDate = DateTime.Today;

            using (EmployeesDbContext dbContext = new EmployeesDbContext(_configuration))
            {
                // показване на рождениците днес
                if (inputModel.ShowBirthdaysToday)
                {
                    employees = dbContext.Employee
                        .Where(e => e.BirthDate.Day == currentDate.Day && e.BirthDate.Month == currentDate.Month)
                        .OrderBy(e => e.BirthDate)
                        .ToList();
                }
                // показване на всички предстоящи рождени дни
                if (inputModel.ShowUpcomingBirthdaysOnly)
                {
                    employees = dbContext.Employee
                        .Where(e => e.BirthDate.Day >= currentDate.Day && e.BirthDate.Month >= currentDate.Month)
                        .OrderBy(e => e.BirthDate)
                        .ToList();
                }
                // показване на всички минали рождени дни
                else if (inputModel.ShowPastBirthdaysOnly)
                {
                    employees = dbContext.Employee
                        .Where(e => e.BirthDate.Day < currentDate.Day && e.BirthDate.Month < currentDate.Month)
                        .OrderBy(e => e.BirthDate)
                        .ToList();
                }
                // показване на рождени дни за период
                else if (inputModel.SearchByBirthdayRange)
                {
                    employees = dbContext.Employee
                        .Where(e => (e.BirthDate.Day >= inputModel.BirthdaysRange.FromDate.Day && e.BirthDate.Month >= inputModel.BirthdaysRange.FromDate.Month)
                                && (e.BirthDate.Day <= inputModel.BirthdaysRange.ToDate.Day && e.BirthDate.Month <= inputModel.BirthdaysRange.ToDate.Month))
                        .OrderBy(e => e.BirthDate)
                        .ToList();
                }
                // ако няма фитриране, се показват вскички рождени дни
                else
                {
                    employees = dbContext.Employee
                        .OrderBy(e => e.BirthDate)
                        .ToList();
                }
            }

            EmployeesBirthdaysOutputModel outputModel = new EmployeesBirthdaysOutputModel();
            employees.ForEach(employee =>
            {
                EmployeeModel employeeModel = EmployeeMapper.Map(employee);
                outputModel.Employees.Add(employeeModel);
            });

            return Ok(outputModel);
        }
    }
}
