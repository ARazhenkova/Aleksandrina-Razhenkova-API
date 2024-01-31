using EmployeesApi.Models.Common;

namespace EmployeesApi.Models.Reports.EmployeesBirthdays
{
    public class EmployeesBirthdaysInputModel
    {
        public bool ShowBirthdaysToday { get; set; } = false;
        public bool ShowUpcomingBirthdaysOnly { get; set; } = false;
        public bool ShowPastBirthdaysOnly { get; set; } = false;

        public bool SearchByBirthdayRange { get; set; } = false;
        public DateRangeModel BirthdaysRange { get; set; } = new DateRangeModel();
    }
}
