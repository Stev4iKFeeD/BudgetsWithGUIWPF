using System;
using System.Collections.Generic;
using System.Linq;
using Budgets.BusinessLayer.Transactions;

namespace Budgets.BusinessLayer.Wallets
{
    public class Wallet
    {
        private decimal _currentWithoutInitialBalance;

        public Guid Guid { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal CurrentBalance
        {
            get => _currentWithoutInitialBalance + InitialBalance;
            private set => _currentWithoutInitialBalance = value;
        }
        public List<Transaction> Transactions { get; }
        public List<Category> Categories { get; }

        public Guid OwnerGuid { get; }


        public Wallet(Guid guid, string name, string description, string currency, decimal initialBalance, Guid ownerGuid)
        {
            Guid = guid;
            Name = name;
            Description = description;
            Currency = currency;
            InitialBalance = initialBalance;

            Transactions = new List<Transaction>();
            Categories = new List<Category>();

            OwnerGuid = ownerGuid;
        }


        // public decimal CurrentBalance()
        // {
        //     decimal res = _initialBalance;
        //     foreach (var transaction in _transactions)
        //         res += transaction.Sum;
        //
        //     return res;
        // }

        private decimal IncomesOrExpenses(DateTimeOffset since, DateTimeOffset to, bool incomes)
        {
            decimal res = 0;
            foreach (var transaction in Transactions)
                if (since <= transaction.Date && transaction.Date <= to)
                    if (incomes && transaction.Sum > 0)
                        res += transaction.Sum;
                    else if (!incomes && transaction.Sum < 0)
                        res += transaction.Sum;

            return incomes ? res : Math.Abs(res);
        }

        public decimal Incomes(DateTimeOffset since, DateTimeOffset to)
        {
            return IncomesOrExpenses(since, to, true);
        }

        public decimal IncomesForCurrentMonth()
        {
            DateTimeOffset now = DateTimeOffset.Now;
            return Incomes(new DateTime(now.Year, now.Month, 1), now);
        }

        public decimal Expenses(DateTimeOffset since, DateTimeOffset to)
        {
            return IncomesOrExpenses(since, to, false);
        }

        public decimal ExpensesForCurrentMonth()
        {
            DateTimeOffset now = DateTimeOffset.Now;
            return Expenses(new DateTime(now.Year, now.Month, 1), now);
        }

        public bool AddTransaction(Transaction transaction)
        {
            if (Categories.Count != 0)
                if (!Categories.Contains(transaction.Category))
                    return false;

            Transactions.Add(transaction);
            _currentWithoutInitialBalance += transaction.Sum;
            return true;
        }

        public bool RemoveTransaction(Transaction transaction)
        {
            _currentWithoutInitialBalance -= transaction.Sum;
            return Transactions.Remove(transaction);
        }

        /**
         * @param index  begins with 1
         */
        public bool RemoveTransaction(int index)
        {
            if (index < 1 || index > Transactions.Count)
                return false;

            _currentWithoutInitialBalance -= Transactions[index - 1].Sum;
            Transactions.RemoveAt(index - 1);
            return true;
        }

        /**
         * @param index  begins with 1
         */
        public Transaction GetTransaction(int index)
        {
            if (index < 1 || index > Transactions.Count)
                return null;

            return Transactions[index - 1];
        }

        /**
         * @param from  begins with 1
         * @param to    inclusive
         */
        public List<Transaction> GetTransactions(int from, int to)
        {
            int firstIndex = Math.Min(from, to) - 1;
            int count = Math.Max(from, to) - firstIndex;
            if (firstIndex < 0 || count > /*_transactions.Count*/ 10)
                return null;
            
            return Transactions.GetRange(firstIndex, count);
        }

        public List<Transaction> GetTenRecentlyAddedTransactions()
        {
            return GetTransactions(Math.Max(1, Transactions.Count - 9), Transactions.Count);
        }

        public bool AddCategory(Category category)
        {
            if (Categories.Contains(category))
                return false;

            Categories.Add(category);
            return true;
        }

        public bool RemoveCategory(Category category)
        {
            return Categories.Remove(category);
        }

        /**
         * @param index  begins with 1
         */
        public bool RemoveCategory(int index)
        {
            if (index < 1 || index > Categories.Count)
                return false;

            Categories.RemoveAt(index - 1);
            return true;
        }

        /**
         * @param index  begins with 1
         */
        public Category GetCategory(int index)
        {
            if (index < 1 || index > Categories.Count)
                return null;

            return Categories[index - 1];
        }

        // /**
        //  * @param from  begins with 1
        //  * @param to    inclusive
        //  */
        // public List<Category> GetCategories(int from, int to)
        // {
        //     int firstIndex = Math.Min(from, to) - 1;
        //     int count = Math.Max(from, to) - firstIndex;
        //     if (firstIndex < 0 || count > Categories.Count)
        //         return null;
        //
        //     return Categories.GetRange(firstIndex, count);
        // }

        public List<Category> GetCategories()
        {
            // return _categories.GetRange(0, _categories.Count);
            return Categories.ToList();
        }

        public bool Validate()
        {
            bool res = true;

            if (string.IsNullOrWhiteSpace(Name))
                res = false;
            if (string.IsNullOrWhiteSpace(Currency))
                res = false;

            return res;
        }
    }
}
