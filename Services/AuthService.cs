using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Budgets.BusinessLayer.User;
using DataStorage;

namespace Budgets.Services
{
    public class AuthService
    {
        private FileDataStorage<DBUser> _storage = new FileDataStorage<DBUser>();

        public async Task<User> Authenticate(AuthUser authUser)
        {
            return await Task.Run(async () =>
            {
                Thread.Sleep(1000);

                if (string.IsNullOrWhiteSpace(authUser.Login) || string.IsNullOrWhiteSpace(authUser.Password))
                {
                    throw new ArgumentException("Login or Password is empty");
                }

                byte[] encryptedPassword = System.Text.Encoding.ASCII.GetBytes(authUser.Password);
                encryptedPassword = new System.Security.Cryptography.SHA256Managed().ComputeHash(encryptedPassword);
                string hashPassword = System.Text.Encoding.ASCII.GetString(encryptedPassword);

                List<DBUser> users = await _storage.GetAllAsync();
                DBUser dbUser =
                    users.FirstOrDefault(user => user.Login == authUser.Login && user.Password == hashPassword);
                if (dbUser == null)
                {
                    throw new Exception("Wrong Login or Password");
                }

                return new User(dbUser.Guid, dbUser.Login, dbUser.FirstName, dbUser.LastName, dbUser.Email);
            });
        }

        public async Task Register(RegUser regUser)
        {
            await Task.Run(async () =>
            {
                Thread.Sleep(1000);

                if (string.IsNullOrWhiteSpace(regUser.Login)
                    || string.IsNullOrWhiteSpace(regUser.Password)
                    || string.IsNullOrWhiteSpace(regUser.FirstName)
                    || string.IsNullOrWhiteSpace(regUser.LastName)
                    || string.IsNullOrWhiteSpace(regUser.Email))
                {
                    throw new ArgumentException("Some fields are empty");
                }

                if (!new EmailAddressAttribute().IsValid(regUser.Email))
                {
                    throw new ArgumentException("Email is invalid");
                }

                List<DBUser> users = await _storage.GetAllAsync();
                DBUser dbUser = users.FirstOrDefault(user => user.Login == regUser.Login);
                if (dbUser != null)
                {
                    throw new Exception("User already exists");
                }

                byte[] encryptedPassword = System.Text.Encoding.ASCII.GetBytes(regUser.Password);
                encryptedPassword = new System.Security.Cryptography.SHA256Managed().ComputeHash(encryptedPassword);
                string hashPassword = System.Text.Encoding.ASCII.GetString(encryptedPassword);

                await _storage.AddOrUpdateAsync(new DBUser
                (
                    regUser.Login,
                    hashPassword,
                    regUser.FirstName,
                    regUser.LastName,
                    regUser.Email
                ));
            });
        }
    }
}
