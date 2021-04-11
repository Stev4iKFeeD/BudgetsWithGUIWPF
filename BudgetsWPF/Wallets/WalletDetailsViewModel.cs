﻿using System;
using System.ComponentModel;
using System.Windows;
using Budgets.BusinessLayer.Wallets;
using Budgets.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace Budgets.GUI.WPF.Wallets
{
    public class WalletDetailsViewModel : BindableBase, IDataErrorInfo
    {
        private Wallet _wallet;
        private WalletsService _walletsService;

        private bool _isEnabled;
        private bool _isIndeterminate;
        private Visibility _visibility;

        private bool _hasChanged;
        private bool _canSave;

        public Guid Guid => _wallet.Guid;

        public string Name
        {
            get => _wallet.Name;
            set
            {
                if (_wallet.Name != value)
                {
                    _wallet.Name = value;
                    _hasChanged = true;
                    _canSave = this[nameof(Name)] == string.Empty;
                    RaisePropertyChanged();
                    SaveWalletCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Description
        {
            get => _wallet.Description;
            set
            {
                if (_wallet.Description != value)
                {
                    _wallet.Description = value;
                    _hasChanged = true;
                    _canSave = true;
                    RaisePropertyChanged();
                    SaveWalletCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Currency
        {
            get => _wallet.Currency;
            set
            {
                if (_wallet.Currency != value)
                {
                    _wallet.Currency = value;
                    _hasChanged = true;
                    _canSave = this[nameof(Currency)] == string.Empty;
                    RaisePropertyChanged();
                    SaveWalletCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public decimal InitialBalance
        {
            get => _wallet.InitialBalance;
            set
            {
                if (_wallet.InitialBalance != value)
                {
                    _wallet.InitialBalance = value;
                    _hasChanged = true;
                    RaisePropertyChanged();
                    SaveWalletCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string DisplayName => $"{_wallet.Name} ({_wallet.InitialBalance})";

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

        public bool HasChanged => _hasChanged;

        public DelegateCommand SaveWalletCommand { get; }

        public WalletDetailsViewModel(Wallet wallet, WalletsService walletsService)
        {
            _hasChanged = false;
            _isEnabled = true;
            _isIndeterminate = false;
            _visibility = Visibility.Hidden;

            _wallet = wallet;
            _walletsService = walletsService;

            SaveWalletCommand = new DelegateCommand(SaveWallet, () => _canSave);
        }

        private async void SaveWallet()
        {
            IsEnabled = false;
            IsIndeterminate = true;
            Visibility = Visibility.Visible;

            await _walletsService.AddOrUpdateWallet(new SaveWallet()
            {
                Guid = _wallet.Guid,
                Name = _wallet.Name,
                Description = _wallet.Description,
                Currency = _wallet.Currency,
                InitialBalance = _wallet.InitialBalance,
                OwnerGuid = _wallet.OwnerGuid
            });

            _hasChanged = false;
            RaisePropertyChanged(nameof(DisplayName));
            SaveWalletCommand.RaiseCanExecuteChanged();

            Visibility = Visibility.Hidden;
            IsIndeterminate = false;
            IsEnabled = true;
        }

        public string Error { get; }

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case "Name":
                        if (string.IsNullOrWhiteSpace(Name))
                        {
                            error = "Name cannot be empty";
                        }
                        break;
                    case "Currency":
                        if (string.IsNullOrWhiteSpace(Currency))
                        {
                            error = "Currency cannot be empty";
                        }
                        break;
                }

                return error;
            }
        }
    }
}
