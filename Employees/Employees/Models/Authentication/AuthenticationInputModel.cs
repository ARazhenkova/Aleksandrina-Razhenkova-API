using System.ComponentModel.DataAnnotations;

namespace EmployeesApi.Models.Authentication
{
    public class AuthenticationInputModel
    {
        [Required]
        public string Email {  get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
