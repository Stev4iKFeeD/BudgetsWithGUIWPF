using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using Budgets.BusinessLayer;
using Budgets.BusinessLayer.Transactions;
using Budgets.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace Budgets.GUI.WPF.Transactions
{
    public class TransactionDetailsViewModel : BindableBase, IDataErrorInfo
    {
        private TransactionsListViewModel _transactionsListViewModel;
        
        private Transaction _transaction;

        private bool _isEnabled;
        private bool _isIndeterminate;
        private Visibility _visibility;

        private bool _canSave;

        public Guid Guid => _transaction.Guid;

        public decimal Sum
        {
            get => _transaction.Sum;
            set
            {
                if (_transaction.Sum != value)
                {
                    _transaction.Sum = value;
                    _canSave = true;
                    RaisePropertyChanged();
                    SaveTransactionCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Currency
        {
            get => _transaction.Currency;
            set
            {
                if (_transaction.Currency != value)
                {
                    _transaction.Currency = value;
                    _canSave = this[nameof(Currency)] == string.Empty;
                    RaisePropertyChanged();
                    SaveTransactionCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public Category Category
        {
            get => _transaction.Category;
            set
            {
                if (_transaction.Category != value)
                {
                    _transaction.Category = value;
                    _canSave = true;
                    RaisePropertyChanged();
                    SaveTransactionCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Description
        {
            get => _transaction.Description;
            set
            {
                if (_transaction.Description != value)
                {
                    _transaction.Description = value;
                    _canSave = true;
                    RaisePropertyChanged();
                    SaveTransactionCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public DateTimeOffset Date
        {
            get => _transaction.Date;
            set
            {
                if (_transaction.Date != value)
                {
                    _transaction.Date = value;
                    _canSave = true;
                    RaisePropertyChanged();
                    SaveTransactionCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string DateString
        {
            get => _transaction.Date.ToString("dd.MM.yyyy HH:mm:ss");
            set
            {
                if (_transaction.Date.ToString("dd.MM.yyyy HH:mm:ss") != value)
                {
                    Date = DateTimeOffset.ParseExact(value, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);
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

        public string TransactionDisplayName => $"{Sum}: {Description} ({Date.LocalDateTime})";

        public DelegateCommand BackToListCommand { get; }
        public DelegateCommand SaveTransactionCommand { get; }
        public DelegateCommand RemoveTransactionCommand { get; }

        public TransactionDetailsViewModel(TransactionsListViewModel transactionsListViewModel, Transaction transaction)
        {
            _canSave = false;
            _isEnabled = true;
            _isIndeterminate = false;
            _visibility = Visibility.Hidden;

            _transactionsListViewModel = transactionsListViewModel;

            _transaction = transaction;

            BackToListCommand = new DelegateCommand(BackToList);
            SaveTransactionCommand = new DelegateCommand(SaveTransaction, () => _canSave);
            RemoveTransactionCommand = new DelegateCommand(RemoveTransaction);
        }

        private void BackToList()
        {
            _transactionsListViewModel.CurrentTransaction = null;
        }

        private void SaveTransaction()
        {
            IsEnabled = false;
            IsIndeterminate = true;
            Visibility = Visibility.Visible;

            _transactionsListViewModel.SaveTransaction();

            _canSave = false;
            RaisePropertyChanged(nameof(TransactionDisplayName));
            SaveTransactionCommand.RaiseCanExecuteChanged();

            Visibility = Visibility.Hidden;
            IsIndeterminate = false;
            IsEnabled = true;
        }

        private void RemoveTransaction()
        {
            IsEnabled = false;
            IsIndeterminate = true;
            Visibility = Visibility.Visible;

            _transactionsListViewModel.RemoveTransaction();

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
