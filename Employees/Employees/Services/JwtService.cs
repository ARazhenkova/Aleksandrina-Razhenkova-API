using EmployeesApi.Data;
using EmployeesApi.ModelToDataMappers;
using EmployeesApi.Settings;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeesApi.Services
{
    public interface IJwtService
    {
        string CreateToken(Employee employee);
    }

    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(Employee employee)
        {
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor();
            tokenDescriptor.Expires = CreateNewExpiration();
            tokenDescriptor.Subject = CreateClaimsIdentity(employee);
            tokenDescriptor.SigningCredentials = CreateSigningCredentials();

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private DateTime CreateNewExpiration()
        {
            double tokenLife = _configuration.GetValue<double>(AppConfigurations.JwtSettings.TOKEN_LIFE);

            return DateTime.Now.AddHours(tokenLife);
        }

        private ClaimsIdentity CreateClaimsIdentity(Employee employee)
        {
            Claim[] claims = new Claim[]
            {
                new Claim(ClaimTypes.Role, AccessLevelMapper.Map(employee.AccessLevel)),
                new Claim(ClaimTypes.UserData, Base64UrlEncoder.Encode(employee.Uid))
            };

            return new ClaimsIdentity(claims);
        }

        private SigningCredentials CreateSigningCredentials()
        {
            string symmetricSecurityKeyPath = $"{AppConfigurations.JwtSettings.GetSectionName()}:{AppConfigurations.JwtSettings.SYMMETRIC_SECURITY_KEY}";
            string? tokenSecurityKey = _configuration.GetValue<string>(symmetricSecurityKeyPath);
            if (tokenSecurityKey == null || string.IsNullOrWhiteSpace(tokenSecurityKey))
                throw new ArgumentException($@"There is no security key configured in ""{AppConfigurations.GetFileName()}"" at ""{symmetricSecurityKeyPath}"".");

            byte[] securityKey = Encoding.ASCII.GetBytes(tokenSecurityKey);
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(securityKey);

            return new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        }
    }
}
