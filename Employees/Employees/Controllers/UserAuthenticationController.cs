using EmployeesApi.Data;
using EmployeesApi.Models.Authentication;
using EmployeesApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesApi.Controllers
{
    [ApiController]
    public class UserAuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserAuthenticationService _userAuthenticationService;

        public UserAuthenticationController(IConfiguration configuration, IUserAuthenticationService userAuthenticationService)
        {
            _configuration = configuration;
            _userAuthenticationService = userAuthenticationService;
        }

        [HttpPost("/api/authentication/employee/v1")]
        public ActionResult<AuthenticationOutputModel> Login([FromBody] AuthenticationInputModel inputModel)
        {
            Employee? employee = _userAuthenticationService.FindEmployeeByEmail(inputModel.Email);
            if (employee == null || _userAuthenticationService.IsEnteredPasswordCorrect(employee, inputModel.Password) == false)
                throw new BadHttpRequestException("Incorrect login credentials.");

            AuthenticationOutputModel outputModel = new AuthenticationOutputModel();
            outputModel.Token = _userAuthenticationService.CreateToken(employee);

            return Ok(outputModel);
        }
    }
}
