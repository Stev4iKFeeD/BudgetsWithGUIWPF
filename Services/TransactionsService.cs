using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Budgets.BusinessLayer.Transactions;
using DataStorage;

namespace Budgets.Services
{
    public class TransactionsService
    {
        private FileDataStorage<DBTransaction> _storage;

        public TransactionsService(Guid walletGuid)
        {
            _storage = new FileDataStorage<DBTransaction>(walletGuid.ToString("N"));
        }

        public async Task<List<Transaction>> GetTransactions()
        {
            return await Task.Run(async () =>
            {
                // Thread.Sleep(1000);

                List<DBTransaction> dbTransactions = (await _storage.GetAllAsync()).OrderBy(transaction => transaction.Date).ToList();
                List<Transaction> res = new List<Transaction>();

                foreach (var dbTransaction in dbTransactions)
                {
                    res.Add(new Transaction
                    (
                        dbTransaction.Guid,
                        dbTransaction.Sum,
                        dbTransaction.Currency,
                        dbTransaction.Category,
                        dbTransaction.Description,
                        dbTransaction.Date
                    ));
                }

                return res;
            });
        }

        public async Task AddOrUpdateTransaction(SaveTransaction saveTransaction)
        {
            await Task.Run(async () =>
            {
                // Thread.Sleep(1000);

                await _storage.AddOrUpdateAsync(new DBTransaction
                (
                    saveTransaction.Guid,
                    saveTransaction.Sum,
                    saveTransaction.Currency,
                    saveTransaction.Category,
                    saveTransaction.Description,
                    saveTransaction.Date
                ));
            });
        }

        public async Task<bool> RemoveTransaction(Guid transactionGuid)
        {
            return await Task.Run(async () =>
            {
                // Thread.Sleep(1000);

                return await _storage.Delete(transactionGuid);
            });
        }
    }
}
