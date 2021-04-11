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

        public DBUser(Guid guid, string login, string password, string firstName, string lastName, string email)
        {
            Guid = guid;
            Login = login;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}
