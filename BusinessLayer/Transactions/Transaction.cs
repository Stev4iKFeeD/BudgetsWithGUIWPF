using System;
using System.Collections.Generic;

namespace Budgets.BusinessLayer.Transactions
{
    public class Transaction
    {
        public Guid Guid { get; private set; }
        public decimal Sum { get; set; }
        public string Currency { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Date { get; set; }
        public List<string> Files { get; set; }


        public Transaction(Guid id, decimal sum, string currency, Category category, string description, DateTimeOffset date)
        {
            Guid = id;
            Sum = sum;
            Currency = currency;
            Category = category;
            Description = description;
            Date = date;

            Files = new List<string>();
        }


        public bool Validate()
        {
            bool res = true;

            if (string.IsNullOrWhiteSpace(Currency))
                res = false;
            if (Category == null)
                res = false;

            return res;
        }
    }
}
