using EmployeesApi.Settings;
using System.Text.RegularExpressions;

namespace EmployeesApi.Services
{
    public interface IEmployeeValidatorService
    {
        void ValidateUid(string uid);
        void ValidateName(string name);
        void ValidateLastName(string lastName);
        void ValidateEmail(string email);
        Task ValidateCountryAlpha2CodeAsync(string countryAlpha2Code);
        void ValidateBirthDate(DateTime birthDate);
        void ValidateDepartmentName(string departmentName);
        void ValidatePositionName(string positionName);
        void ValidateAccessLevel(int accessLevel);
        void ValidatePassword(string password);
        void ValidateSecurePassword(string password, string salt);
    }

    public class EmployeeValidatorService : IEmployeeValidatorService
    {
        private const string NAME_ALLOWED_SYMBOLS = "^[a-zA-Zа-яА-Я]+$";
        private const string LAST_NAME_ALLOWED_SYMBOLS = "^[a-zA-Zа-яА-Я]+$";
        private const string EMAIL_ALLOWED_SYMBOLS = "^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$";

        private const int UID_LENGTH = 36;
        private const int NAME_MAX_LENGTH = 128;
        private const int LAST_NAME_MAX_LENGTH = 128;
        private const int EMAIL_MAX_LENGTH = 128;
        private const int COUNTRY_ALPHA_2_CODE_LENGTH = 2;
        private const int DEPARTMENT_NAME_MAX_LENGTH = 64;
        private const int POSITION_NAME_MAX_LENGTH = 64;
        private const int PASSWORD_MAX_LENGTH = 128;
        private const int SECURE_PASSWORD_MAX_LENGTH = 256;
        private const int SALT_MAX_LENGTH = 64;

        private readonly IConfiguration _configuration;
        private readonly ICountriesService _countriesService;

        public EmployeeValidatorService(IConfiguration configuration, ICountriesService countriesService)
        {
            _configuration = configuration;
            _countriesService = countriesService;
        }

        public void ValidateUid(string uid)
        {
            if (string.IsNullOrWhiteSpace(uid))
                throw new ArgumentException("The field \"uid\" is required.");

            if (uid.Length != UID_LENGTH)
                throw new ArgumentException($"Invalid UID.");
        }

        public void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The field \"name\" is required.");

            if (name.Length > NAME_MAX_LENGTH)
                throw new ArgumentException($"The field \"name\" must be less than {NAME_MAX_LENGTH} symbols.");

            Regex regex = new Regex(NAME_ALLOWED_SYMBOLS);
            if (regex.IsMatch(name) == false)
                throw new ArgumentException("The field \"name\" has invalid symbols.");
        }

        public void ValidateLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("The field \"last name\" is required.");

            if (lastName.Length > LAST_NAME_MAX_LENGTH)
                throw new ArgumentException($"The field \"last name\" must be less than {LAST_NAME_MAX_LENGTH} symbols.");

            Regex regex = new Regex(LAST_NAME_ALLOWED_SYMBOLS);
            if (regex.IsMatch(lastName) == false)
                throw new ArgumentException("The field \"last name\" has invalid symbols.");
        }

        public void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("The field \"email\" is required.");

            if (email.Length > EMAIL_MAX_LENGTH)
                throw new ArgumentException($"The field \"email\" must be less than {EMAIL_MAX_LENGTH} symbols.");

            Regex regex = new Regex(EMAIL_ALLOWED_SYMBOLS);
            if (regex.IsMatch(email) == false)
                throw new ArgumentException("The field \"email\" has invalid symbols.");
        }

        public async Task ValidateCountryAlpha2CodeAsync(string countryAlpha2Code)
        {
            if (string.IsNullOrWhiteSpace(countryAlpha2Code))
                throw new ArgumentException("The field \"country alpha-2 code\" is required.");

            if (countryAlpha2Code.Length != COUNTRY_ALPHA_2_CODE_LENGTH)
                throw new ArgumentException($"The field \"country alpha-2 code\" must be {COUNTRY_ALPHA_2_CODE_LENGTH} symbols.");

            if (await _countriesService.FindByAlpha2CodeAsync(countryAlpha2Code) == null)
                throw new ArgumentException("Invalid country alpha-2 code.");
        }

        public void ValidateBirthDate(DateTime birthDate)
        {
            if (birthDate > DateTime.Today)
                throw new ArgumentException($"Invalid birth date.");

            int? legalWorkingAge = _configuration.GetValue<int>(AppConfigurations.LEGAL_WORKING_AGE);
            if (legalWorkingAge == null)
                throw new ArgumentException($@"The legal working age is not configured in ""{AppConfigurations.GetFileName()}"" at ""{AppConfigurations.LEGAL_WORKING_AGE}"".");

            if (GetAgeByBirthDate(birthDate) < legalWorkingAge)
                throw new ArgumentException($"Legal working age is {legalWorkingAge}.");
        }

        public void ValidateDepartmentName(string departmentName)
        {
            if (string.IsNullOrWhiteSpace(departmentName))
                throw new ArgumentException("The field \"department name\" is required.");

            if (departmentName.Length > DEPARTMENT_NAME_MAX_LENGTH)
                throw new ArgumentException($"The field \"department name\" must be less than {DEPARTMENT_NAME_MAX_LENGTH} symbols.");
        }

        public void ValidatePositionName(string positionName)
        {
            if (string.IsNullOrWhiteSpace(positionName))
                throw new ArgumentException("The field \"position name\" is required.");

            if (positionName.Length > POSITION_NAME_MAX_LENGTH)
                throw new ArgumentException($"The field \"position name\" must be less than {POSITION_NAME_MAX_LENGTH} symbols.");
        }

        public void ValidateAccessLevel(int accessLevel)
        {
            if (accessLevel < AccessLevels.FISRT_ALLOWED_INDEX || accessLevel > AccessLevels.MAX_USED_INDEX)
                throw new ArgumentException($"Invalid access level.");
        }

        public void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("The field \"password\" is required.");

            if (password.Length > PASSWORD_MAX_LENGTH)
                throw new ArgumentException($"The field \"password\" must be less than {PASSWORD_MAX_LENGTH} symbols.");
        }

        public void ValidateSecurePassword(string password, string salt)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("The field \"password\" is required.");

            if (password.Length > SECURE_PASSWORD_MAX_LENGTH)
                throw new ArgumentException($"The field \"password\" must be less than {SECURE_PASSWORD_MAX_LENGTH} symbols.");

            if (string.IsNullOrWhiteSpace(salt))
                throw new ArgumentException("The field \"salt\" is required.");

            if (salt.Length > SALT_MAX_LENGTH)
                throw new ArgumentException($"The field \"salt\" must be less than {SALT_MAX_LENGTH} symbols.");
        }

        private int GetAgeByBirthDate(DateTime birthDate)
        {
            int age = DateTime.Today.Year - birthDate.Year;

            if (birthDate.CompareTo(DateTime.Today.AddYears(-age)) > 0)
                age--;

            return age;
        }
    }
}
