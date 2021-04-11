using System;

namespace Budgets.BusinessLayer.Wallets
{
    public class SaveWallet
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public decimal InitialBalance { get; set; }

        public Guid OwnerGuid { get; set; }
    }
}
