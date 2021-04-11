using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Budgets.BusinessLayer.Wallets;
using Budgets.GUI.WPF.Navigation;
using Budgets.Services;
using Prism.Mvvm;

namespace Budgets.GUI.WPF.Wallets
{
    public class WalletsViewModel : BindableBase, INavigatable<MainNavigatableType>
    {
        private WalletsService _walletsService;
        private WalletDetailsViewModel _currentWallet;

        public ObservableCollection<WalletDetailsViewModel> Wallets { get; set; }

        public WalletDetailsViewModel CurrentWallet
        {
            get => _currentWallet;
            set
            {
                _currentWallet = value;
                RaisePropertyChanged();
            }
        }


        public WalletsViewModel(Action logOut)
        {
            _walletsService = new WalletsService();
            Wallets = new ObservableCollection<WalletDetailsViewModel>();
            List<Wallet> loadedWallets = _walletsService.GetWallets().Result;
            foreach (var wallet in loadedWallets)
            {
                Wallets.Add(new WalletDetailsViewModel(wallet));
            }
            
            // TODO Implement LogOut
        }

        public MainNavigatableType Type => MainNavigatableType.Wallets;
        public void ClearSensitiveData()
        {
            
        }
    }
}
