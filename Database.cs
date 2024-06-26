﻿using CBS_ASP.NET_Core_Course_Project.Models;

namespace CBS_ASP.NET_Core_Course_Project
{
    public static class Database
    {
        public static List<User> Users { get; set; } = new List<User>();
        public static List<string> GetEmailsWithSendEmailsOn()
        {
            return Users.Where(u => u.sendEmails).Select(u => u.Login).ToList();
        }
        public static bool? GetSendEmailsStatusByEmail(string email)
        {
            var user = Users.FirstOrDefault(u => u.Login.Equals(email, StringComparison.OrdinalIgnoreCase));

            return user?.sendEmails;
        }
        public static User GetUserByLogin(string login)
        {
            return Users.FirstOrDefault(u => u.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
        }
    }
}
