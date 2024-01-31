using EmployeesApi.Data;
using EmployeesApi.Settings;
using Microsoft.EntityFrameworkCore;

namespace EmployeesApi.Entities
{
    public class EmployeesDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public EmployeesDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<Employee> Employee { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string? connectionString = _configuration.GetConnectionString(AppConfigurations.ConnectionStrings.EMPLOYEES);
            if (connectionString == null || String.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException($@"The connection string for employees databse is not configured in the ""{AppConfigurations.GetFileName()}"" at ""{AppConfigurations.ConnectionStrings.EMPLOYEES}"".");

            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
