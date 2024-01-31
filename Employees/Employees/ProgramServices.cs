using EmployeesApi.Services;
using EmployeesApi.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace EmployeesApi
{
    public static class ProgramServices
    {
        public static IServiceCollection AddSystemServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpClient();
            services.AddMemoryCache();
            services.AddProblemDetails();

            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ISecurePasswordService, SecurePasswordService>();

            services.AddSingleton<ICountriesService, CountriesService>();

            services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeValidatorService, EmployeeValidatorService>();

            return services;
        }

        public static IServiceCollection AddJwtServices(this IServiceCollection services, IConfiguration configuration)
        {
            string symmetricSecurityKeyPath = $"{AppConfigurations.JwtSettings.GetSectionName()}:{AppConfigurations.JwtSettings.SYMMETRIC_SECURITY_KEY}";
            string? symmetricSecurityKey = configuration.GetValue<string>(symmetricSecurityKeyPath);
            if (symmetricSecurityKey == null || String.IsNullOrWhiteSpace(symmetricSecurityKey))
                throw new ArgumentException($@"There is no security key configured in ""{AppConfigurations.GetFileName()}"" at ""{symmetricSecurityKeyPath}"".");

            AuthenticationBuilder authenticationBuilder = services.AddAuthentication(authentication =>
            {
                authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            authenticationBuilder.AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(symmetricSecurityKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return services;
        }

        public static IServiceCollection AddOpenApiServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "EmployeesApi", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Please enter token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            return services;
        }
    }
}
