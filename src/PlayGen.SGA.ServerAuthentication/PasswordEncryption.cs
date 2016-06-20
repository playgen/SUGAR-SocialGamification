using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PlayGen.SGA.ServerAuthentication
{
    public class PasswordEncryption
    {
        public string Encrypt(string password, string salt)
        {
            using (var hashAlgorithm = SHA256.Create())
            {
                var saltedPassword = $"{salt}{password}";
                var saltedPasswordAsBytes = Encoding.UTF8.GetBytes(saltedPassword);
                var saltedPasswordHash = hashAlgorithm.ComputeHash(saltedPasswordAsBytes);
                return Convert.ToBase64String(saltedPasswordHash);
            }
        }

        public string CreateSalt()
        {
            var data = new byte[0x10];
            using (var cryptoServiceProvider = RandomNumberGenerator.Create())
            {
                cryptoServiceProvider.GetBytes(data);
                return Convert.ToBase64String(data);
            }
        }
    }
}
