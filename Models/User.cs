namespace CBS_ASP.NET_Core_Course_Project.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public bool sendEmails { get; set; }
    }
}
