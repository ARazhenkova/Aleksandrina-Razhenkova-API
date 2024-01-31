using System.Security.Cryptography;
using System.Text;

namespace EmployeesApi.Services
{
    public interface ISecurePasswordService
    {
        string CreateHashWithRandomSalt(string password);
        string RecreateHash(string password, string salt);
        string GetLastGeneratedSalt();
    }

    public class SecurePasswordService : ISecurePasswordService
    {
        private const int SALT_SIZE = 16;
        private const int HASH_SIZE = 128;
        private const int INTERATIONS = 10000;

        private string _salt = string.Empty;

        public SecurePasswordService()
        {

        }

        public string CreateHashWithRandomSalt(string password)
        {
            byte[] rawSalt = GenerateRandomSalt();
            byte[] rawHash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), rawSalt, INTERATIONS, HashAlgorithmName.SHA512, HASH_SIZE);

            return Convert.ToBase64String(rawHash);
        }

        public string RecreateHash(string password, string salt)
        {
            byte[] rawSalt = Convert.FromBase64String(salt);
            byte[] rawHash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), rawSalt, INTERATIONS, HashAlgorithmName.SHA512, HASH_SIZE);

            return Convert.ToBase64String(rawHash);
        }

        public string GetLastGeneratedSalt()
        {
            return _salt;
        }

        private byte[] GenerateRandomSalt()
        {
            byte[] rawSalt = new byte[SALT_SIZE];

            using (RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetNonZeroBytes(rawSalt);
            }

            _salt = Convert.ToBase64String(rawSalt);

            return rawSalt;
        }
    }
}
