using System.Security.Cryptography;
using System.Text;
using CBS_ASP.NET_Core_Course_Project.Models;

namespace CBS_ASP.NET_Core_Course_Project
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            var byted = Encoding.UTF8.GetBytes(password);
            var sha1 = SHA1CryptoServiceProvider.Create();
            var hashedBytes = sha1.ComputeHash(byted);
            return Encoding.UTF8.GetString(hashedBytes);
        }

        public static bool IsCorrectPassword(User user, string password)
        {
            var passwordHash = HashPassword(password);
            return passwordHash == user.PasswordHash;
        }
    }
}
