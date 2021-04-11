using System;
using DataStorage;

namespace Budgets.BusinessLayer.User
{
    public class DBUser : IStorable
    {
        public Guid Guid { get; }
        public string Login { get; }
        public string Password { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }

        public DBUser(string login, string password, string firstName, string lastName, string email)
        {
            Guid = Guid.NewGuid();
            Login = login;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}
