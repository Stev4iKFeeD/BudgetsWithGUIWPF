using Budgets.BusinessLayer.Wallets;
using Prism.Mvvm;

namespace Budgets.GUI.WPF.Wallets
{
    public class WalletDetailsViewModel : BindableBase
    {
        private Wallet _wallet;

        public string Name
        {
            get => _wallet.Name;
            set
            {
                _wallet.Name = value;
                RaisePropertyChanged(nameof(DisplayName));
            }
        }

        public decimal Balance
        {
            get => _wallet.InitialBalance;
            set
            {
                _wallet.InitialBalance = value;
                RaisePropertyChanged(nameof(DisplayName));
            }
        }

        public string DisplayName => $"{_wallet.Name} ({_wallet.InitialBalance})";


        public WalletDetailsViewModel(Wallet wallet)
        {
            _wallet = wallet;
        }
    }
}
