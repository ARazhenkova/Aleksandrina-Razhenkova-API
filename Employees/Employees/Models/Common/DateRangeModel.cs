namespace EmployeesApi.Models.Common
{
    public class DateRangeModel
    {
        public DateTime FromDate { get; set; } = DateTime.Today;
        public DateTime ToDate { get; set; } = DateTime.Today;
    }
}
