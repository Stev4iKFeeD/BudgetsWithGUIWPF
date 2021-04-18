using System;

namespace Budgets.BusinessLayer.Transactions
{
    public class SaveTransaction
    {
        public Guid Guid { get; set; }
        public decimal Sum { get; set; }
        public string Currency { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
