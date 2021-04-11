using System;
using System.Collections.Generic;
using Budgets.BusinessLayer.Wallets;

namespace Budgets.BusinessLayer
{
    public class SharedWallet
    {
        private Wallet _origWallet;

        public SharedWallet(Wallet orig)
        {
            _origWallet = orig;
        }

        public Guid Guid
        {
            get => _origWallet.Guid;
        }

        public string Name
        {
            get => _origWallet.Name;
        }

        public string Description
        {
            get => _origWallet.Description;
        }

        public string Currency
        {
            get => _origWallet.Currency;
        }

        public decimal InitialBalance
        {
            get => _origWallet.InitialBalance;
        }

        public decimal CurrentBalance
        {
            get => _origWallet.CurrentBalance;
        }

        public decimal Incomes(DateTime since, DateTime to)
        {
            return _origWallet.Incomes(since, to);
        }

        public decimal IncomesForCurrentMonth()
        {
            return _origWallet.IncomesForCurrentMonth();
        }

        public decimal Expenses(DateTime since, DateTime to)
        {
            return _origWallet.Expenses(since, to);
        }

        public decimal ExpensesForCurrentMonth()
        {
            return _origWallet.ExpensesForCurrentMonth();
        }

        public bool AddTransaction(Transaction transaction)
        {
            return _origWallet.AddTransaction(transaction);
        }

        public bool RemoveTransaction(Transaction transaction)
        {
            return _origWallet.RemoveTransaction(transaction);
        }

        /**
         * @param index  begins with 1
         */
        public bool RemoveTransaction(int index)
        {
            return _origWallet.RemoveTransaction(index);
        }

        /**
         * @param index  begins with 1
         */
        public Transaction GetTransaction(int index)
        {
            return _origWallet.GetTransaction(index);
        }

        /**
         * @param from  begins with 1
         * @param to    inclusive
         */
        public List<Transaction> GetTransactions(int from, int to)
        {
            return _origWallet.GetTransactions(from, to);
        }

        public List<Transaction> GetTenRecentlyAddedTransactions()
        {
            return _origWallet.GetTenRecentlyAddedTransactions();
        }

        /**
         * @param index  begins with 1
         */
        public Category GetCategory(int index)
        {
            Category toRes = _origWallet.GetCategory(index);
            return new Category(toRes.Id)
                {Name = toRes.Name, Color = toRes.Color, Description = toRes.Description, Icon = toRes.Icon};
        }

        public List<Category> GetCategories()
        {
            List<Category> origCategories = _origWallet.GetCategories();
            List<Category> toRes = new List<Category>();
            foreach (var origCategory in origCategories)
            {
                toRes.Add(new Category(origCategory.Id)
                {
                    Name = origCategory.Name, Color = origCategory.Color, Description = origCategory.Description,
                    Icon = origCategory.Icon
                });
            }

            return toRes;
        }

        public bool Validate()
        {
            return _origWallet.Validate();
        }
    }
}
