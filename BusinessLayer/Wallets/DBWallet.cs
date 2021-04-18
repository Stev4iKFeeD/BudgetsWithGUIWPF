using System;
using DataStorage;

namespace Budgets.BusinessLayer.Wallets
{
    public class DBWallet : IStorable
    {
        public Guid Guid { get; }
        public string Name { get; }
        public string Description { get; }
        public string Currency { get; }
        public decimal InitialBalance { get; }

        public Guid OwnerGuid { get; }

        public DBWallet(Guid guid, string name, string description, string currency, decimal initialBalance, Guid ownerGuid)
        {
            Guid = guid;
            Name = name;
            Description = description;
            Currency = currency;
            InitialBalance = initialBalance;

            OwnerGuid = ownerGuid;
        }
    }
}
