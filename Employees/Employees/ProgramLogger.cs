using Serilog;
using Serilog.Core;

namespace EmployeesApi
{
    public static class ProgramLogger
    {
        public static ILoggingBuilder AddNewLogger(this ILoggingBuilder logging, IConfiguration configuration)
        {
            logging.ClearProviders();

            Logger logger = new LoggerConfiguration()
              .ReadFrom.Configuration(configuration)
              .Enrich.FromLogContext()
              .CreateLogger();

            logging.AddSerilog(logger);

            return logging;
        }
    }
}
