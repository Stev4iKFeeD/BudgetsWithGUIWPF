using System;
using DataStorage;

namespace Budgets.BusinessLayer.Wallets
{
    public class DBWallet : IStorable
    {
        public Guid Guid { get; }
        public string Name { get; }
        public decimal InitialBalance { get; }

        public DBWallet(string name, decimal initialBalance)
        {
            Guid = Guid.NewGuid();
            Name = name;
            InitialBalance = initialBalance;
        }
    }
}
