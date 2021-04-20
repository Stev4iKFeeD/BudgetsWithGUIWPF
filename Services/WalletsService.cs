using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Budgets.BusinessLayer.Wallets;
using DataStorage;

namespace Budgets.Services
{
    public class WalletsService
    {
        private FileDataStorage<DBWallet> _storage = new FileDataStorage<DBWallet>(CurrentInfo.UserGuid.ToString("N"));
        
        public async Task<List<Wallet>> GetWallets()
        {
            return await Task.Run(async () =>
            {
                // Thread.Sleep(1000);

                List<DBWallet> dbWallets = await _storage.GetAllAsync();
                List<Wallet> res = new List<Wallet>();

                foreach (var dbWallet in dbWallets)
                {
                    res.Add(new Wallet
                    (
                        dbWallet.Guid,
                        dbWallet.Name,
                        dbWallet.Description,
                        dbWallet.Currency,
                        dbWallet.InitialBalance,
                        dbWallet.OwnerGuid
                    ));
                }

                return res;
            });
        }

        public async Task AddOrUpdateWallet(SaveWallet saveWallet)
        {
            await Task.Run(async () =>
            {
                // Thread.Sleep(1000);

                await _storage.AddOrUpdateAsync(new DBWallet
                (
                    saveWallet.Guid, 
                    saveWallet.Name,
                    saveWallet.Description,
                    saveWallet.Currency,
                    saveWallet.InitialBalance,
                    saveWallet.OwnerGuid
                ));
            });
        }

        public async Task<bool> RemoveWallet(Guid walletGuid)
        {
            return await Task.Run(async () =>
            {
                // Thread.Sleep(1000);

                return await _storage.Delete(walletGuid);
            });
        }
    }
}
