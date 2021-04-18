using System;
using DataStorage;

namespace Budgets.BusinessLayer.Transactions
{
    public class DBTransaction : IStorable
    {
        public Guid Guid { get; }
        public decimal Sum { get; }
        public string Currency { get; }
        public Category Category { get; }
        public string Description { get; }
        public DateTimeOffset Date { get; }

        public DBTransaction(Guid guid, decimal sum, string currency, Category category, string description, DateTimeOffset date)
        {
            Guid = guid;
            Sum = sum;
            Currency = currency;
            Category = category;
            Description = description;
            Date = date;
        }
    }
}
