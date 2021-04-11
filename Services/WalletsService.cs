using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Budgets.BusinessLayer.Wallets;
using DataStorage;

namespace Budgets.Services
{
    public class WalletsService
    {
        private List<Wallet> _storage = new List<Wallet>()
        {
            new Wallet(Guid.NewGuid(), "wal1", "desc1", "UAH", 55.01m),
            new Wallet(Guid.NewGuid(), "wal2", "desc2", "UAH", 155.02m),
            new Wallet(Guid.NewGuid(), "wal3", "desc3", "UAH", 255.03m),
            new Wallet(Guid.NewGuid(), "wal4", "desc4", "UAH", 355.04m),
            new Wallet(Guid.NewGuid(), "wal5", "desc5", "UAH", 455.05m)
        };

        public async Task<List<Wallet>> GetWallets()
        {
            // return await Task.Run(async () =>
            // {
            //     Thread.Sleep(1000);
            //
            //     if (userGuid == Guid.Empty)
            //     {
            //         throw new ArgumentException("Bad UserId");
            //     }
            //
            //     List<DBWallet> dbWallets = await new FileDataStorage<DBWallet>(userGuid.ToString("N")).GetAllAsync();
            //     List<Wallet> res = new List<Wallet>();
            //
            //     foreach (var dbWallet in dbWallets)
            //     {
            //         res.Add(new Wallet
            //         (
            //             dbWallet.Guid,
            //             dbWallet.Name,
            //             dbWallet.Name + " desc",
            //             dbWallet.Name + " currency",
            //             dbWallet.InitialBalance
            //         ));
            //     }
            //
            //     return res;
            // });

            return await Task.Run(async () => _storage.ToList());
        }

        // public async Task Register(RegUser regUser)
        // {
        //     Thread.Sleep(1000);
        //
        //     if (string.IsNullOrWhiteSpace(regUser.Login)
        //         || string.IsNullOrWhiteSpace(regUser.Password)
        //         || string.IsNullOrWhiteSpace(regUser.FirstName)
        //         || string.IsNullOrWhiteSpace(regUser.LastName)
        //         || string.IsNullOrWhiteSpace(regUser.Email))
        //     {
        //         throw new ArgumentException("Some fields are empty");
        //     }
        //
        //     List<DBUser> users = await _storage.GetAllAsync();
        //     DBUser dbUser = users.FirstOrDefault(user => user.Login == regUser.Login);
        //     if (dbUser != null)
        //     {
        //         throw new Exception("User already exists");
        //     }
        //
        //     byte[] encryptedPassword = System.Text.Encoding.ASCII.GetBytes(regUser.Password);
        //     encryptedPassword = new System.Security.Cryptography.SHA256Managed().ComputeHash(encryptedPassword);
        //     string hashPassword = System.Text.Encoding.ASCII.GetString(encryptedPassword);
        //
        //     await _storage.AddOrUpdateAsync(new DBUser
        //     (
        //         regUser.Login,
        //         hashPassword,
        //         regUser.FirstName,
        //         regUser.LastName,
        //         regUser.Email
        //     ));
        // }
    }
}
