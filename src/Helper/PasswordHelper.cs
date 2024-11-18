using System;
using System.Security.Cryptography;
using System.Text;

namespace AFFA.src.Helper
{
    public static class PasswordHelper
    {
        public static void CreatePasswordHash(string password, out string hash, out string salt)
        {
            using (var hmac = new HMACSHA512())
            {
                salt = Convert.ToBase64String(hmac.Key);
                hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }



        public static bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
        {
            using (var hmac = new HMACSHA512(Convert.FromBase64String(storedSalt)))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return storedHash == Convert.ToBase64String(computedHash);
            }
        }


    }
}
