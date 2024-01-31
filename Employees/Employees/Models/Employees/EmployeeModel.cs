using System.ComponentModel.DataAnnotations;

namespace EmployeesApi.Models.Employees
{
    public class EmployeeModel
    {
        private DateTime _birthDate = DateTime.Today;

        public string uid { get; set; } = string.Empty;

        [Required]
        public string name { get; set; } = string.Empty;

        [Required]
        public string lastName { get; set; } = string.Empty;

        [Required]
        public string email { get; set; } = string.Empty;

        [Required]
        public string countryAlpha2Code { get; set; } = string.Empty;

        [Required]
        public DateTime birthDate 
        {
            get { return _birthDate.Date; }
            set { _birthDate = value.Date; }
        }

        [Required]
        public string departmentName { get; set; } = string.Empty;

        [Required]
        public string positionName { get; set; } = string.Empty;

        [Required]
        public string accessLevel { get; set; } = string.Empty;

        public string password { get; set; } = string.Empty;
    }
}
