using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeesApi.Data
{
    [Table("EMPLOYEES")]
    public class Employee
    {
        private DateTime _birthDate = DateTime.Today;

        public Employee()
        {

        }

        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; } = 0;

        [Column("ROW_VERSION")]
        [Timestamp]
        public byte[] RowVersion { get; set; } = new byte[0];

        /// <summary> 
        /// UID, с което да се достъпват данните, с цел да не може да се научка вседващия идентификатор.
        /// </summary>
        [Column("UID")]
        public string Uid { get; set; } = string.Empty;

        [Column("NAME")]
        public string Name { get; set;} = string.Empty;

        [Column("LAST_NAME")]
        public string LastName { get; set;} = string.Empty;

        [Column("EMAIL")]
        public string Email { get; set;} = string.Empty;

        [Column("COUNTRY_ALPHA_2_CODE")]
        public string CountryAlpha2Code {  get; set;} = string.Empty;

        [Column("BIRTH_DATE")]
        public DateTime BirthDate
        {
            get { return _birthDate.Date; }
            set { _birthDate = value.Date; }
        }

        [Column("DEPARTMENT_NAME")]
        public string DepartmentName {  get; set;} = string.Empty;

        [Column("POSITION_NAME")]
        public string PositionName {  get; set;} = string.Empty;

        [Column("ACCESS_LEVEL")]
        public int AccessLevel {  get; set;} = 0;

        [Column("PASSWORD")]
        public string Password {  get; set;} = string.Empty;

        [Column("SALT")]
        public string Salt { get; set;} = string.Empty;

        public void Update(Employee employee)
        {
            Name                = employee.Name;
            LastName            = employee.LastName;
            Email               = employee.Email;
            CountryAlpha2Code   = employee.CountryAlpha2Code;
            BirthDate           = employee.BirthDate;
            DepartmentName      = employee.DepartmentName;
            PositionName        = employee.PositionName;
            AccessLevel         = employee.AccessLevel;
        }
    }
}
