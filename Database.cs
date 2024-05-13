using CBS_ASP.NET_Core_Course_Project.Models;

namespace CBS_ASP.NET_Core_Course_Project
{
    public static class Database
    {
        public static List<User> Users { get; set; } = new List<User>()
        {
            new User()
            {
                Id = Guid.NewGuid(),
                Login = "alexxbrawler@gmail.com",
                PasswordHash = PasswordHasher.HashPassword("qwerty")
            }
        };
        public static List<string> GetAllLogins()
        {
            return Users.Select(u => u.Login).ToList();
        }
    }
}
