using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Budgets.BusinessLayer.Wallets;
using Budgets.GUI.WPF.Navigation;
using Budgets.GUI.WPF.Transactions;
using Budgets.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace Budgets.GUI.WPF.Wallets
{
    public class WalletsViewModel : BindableBase, INavigatable<MainNavigatableType>
    {
        private WalletsService _walletsService;
        private WalletDetailsViewModel _currentWalletDetails;
        private TransactionsListViewModel _currentTransactions;

        private bool _isEnabled;
        private bool _isIndeterminate;
        private Visibility _visibility;

        public ObservableCollection<WalletDetailsViewModel> Wallets { get; set; }

        public WalletDetailsViewModel CurrentWalletDetails
        {
            get => _currentWalletDetails;
            set
            {
                if (_currentWalletDetails != value)
                {
                    IsEnabled = false;
                    IsIndeterminate = true;
                    Visibility = Visibility.Visible;

                    _currentWalletDetails = value;

                    Task.Run(() =>
                    {
                        CurrentTransactions = _currentWalletDetails == null
                            ? null
                            : new TransactionsListViewModel(_currentWalletDetails);

                        RaisePropertyChanged();
                        RaisePropertyChanged(nameof(IsWalletSelected));
                        RaisePropertyChanged(nameof(OnWalletSelection));

                        Visibility = Visibility.Hidden;
                        IsIndeterminate = false;
                        IsEnabled = true;
                    });

                    RemoveWalletCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public TransactionsListViewModel CurrentTransactions
        {
            get => _currentTransactions;
            set
            {
                if (_currentTransactions != value)
                {
                    _currentTransactions = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool IsIndeterminate
        {
            get => _isIndeterminate;
            set
            {
                if (_isIndeterminate != value)
                {
                    _isIndeterminate = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Visibility Visibility
        {
            get => _visibility;
            set
            {
                if (_visibility != value)
                {
                    _visibility = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsWalletSelected => CurrentWalletDetails != null;

        public Visibility OnWalletSelection
        {
            get
            {
                if (IsWalletSelected)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Hidden;
                }
            }
        }

        public DelegateCommand AddWalletCommand { get; }
        public DelegateCommand RemoveWalletCommand { get; }
        public DelegateCommand SignOutCommand { get; }

        public WalletsViewModel(Action signOut)
        {
            _isEnabled = true;
            _isIndeterminate = false;
            _visibility = Visibility.Hidden;

            AddWalletCommand = new DelegateCommand(AddWallet);
            RemoveWalletCommand = new DelegateCommand(RemoveWallet, () => CurrentWalletDetails != null);
            SignOutCommand = new DelegateCommand(() => SignOut(signOut));

            ClearSensitiveData();
        }

        private async void AddWallet()
        {
            IsEnabled = false;
            IsIndeterminate = true;
            Visibility = Visibility.Visible;

            Wallet newWallet = new Wallet(Guid.NewGuid(), "Wallet", "", "UAH", 00.00m, CurrentInfo.UserGuid);

            await _walletsService.AddOrUpdateWallet(new SaveWallet()
            {
                Guid = newWallet.Guid,
                Name = newWallet.Name,
                Description = newWallet.Description,
                Currency = newWallet.Currency,
                InitialBalance = newWallet.InitialBalance,
                OwnerGuid = newWallet.OwnerGuid
            });

            WalletDetailsViewModel newWalletDetailsViewModel = new WalletDetailsViewModel(newWallet, _walletsService);
            Wallets.Add(newWalletDetailsViewModel);
            CurrentWalletDetails = newWalletDetailsViewModel;

            // Visibility = Visibility.Hidden;
            // IsIndeterminate = false;
            // IsEnabled = true;
        }

        private async void RemoveWallet()
        {
            IsEnabled = false;

            MessageBoxResult boxResult =
                MessageBox.Show("Are you sure?", "Remove", MessageBoxButton.YesNo);
            if (boxResult == MessageBoxResult.Yes)
            {
                IsIndeterminate = true;
                Visibility = Visibility.Visible;

                if (await _walletsService.RemoveWallet(CurrentWalletDetails.Guid))
                {
                    CurrentTransactions.RemoveAllTransactionsCommand.Execute();
                    int index = Wallets.IndexOf(CurrentWalletDetails);
                    Wallets.RemoveAt(index);
                    // CurrentWalletDetails = index == Wallets.Count
                    //     ? index == 0 
                    //         ? null 
                    //         : Wallets.ElementAt(index - 1)
                    //     : Wallets.ElementAt(index);
                    CurrentWalletDetails = null;

                    // CurrentTransactions = CurrentWalletDetails == null
                    //     ? null
                    //     : new TransactionsListViewModel(_currentWalletDetails.Guid);
                }
            }
            else
            {
                IsEnabled = true;
            }

            // Visibility = Visibility.Hidden;
            // IsIndeterminate = false;
            // IsEnabled = true;
        }

        private void SignOut(Action signOut)
        {
            IsEnabled = false;

            WalletDetailsViewModel unsavedWallet = Wallets.FirstOrDefault(wallet => wallet.HasChanged);
            if (unsavedWallet != null)
            {
                CurrentWalletDetails = unsavedWallet;
                MessageBoxResult boxResult =
                    MessageBox.Show( "Some changes was not saved. Continue?", "Sign Out", MessageBoxButton.YesNo);
                if (boxResult == MessageBoxResult.Yes)
                {
                    signOut.Invoke();
                }
            }
            else
            {
                MessageBoxResult boxResult =
                    MessageBox.Show("Are you sure?", "Sign Out", MessageBoxButton.YesNo);
                if (boxResult == MessageBoxResult.Yes)
                {
                    signOut.Invoke();
                }
            }
            
            IsEnabled = true;
        }

        public MainNavigatableType Type => MainNavigatableType.Wallets;
        public void ClearSensitiveData()
        {
            _walletsService = new WalletsService();
            Wallets = new ObservableCollection<WalletDetailsViewModel>();
            CurrentWalletDetails = null;
            // CurrentTransactions = null;
            List<Wallet> loadedWallets = Task.Run(_walletsService.GetWallets).Result;
            foreach (var wallet in loadedWallets)
            {
                Wallets.Add(new WalletDetailsViewModel(wallet, _walletsService));
            }
        }
    }
}
