using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using Budgets.BusinessLayer;
using Budgets.BusinessLayer.Transactions;
using Budgets.GUI.WPF.Wallets;
using Budgets.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace Budgets.GUI.WPF.Transactions
{
    public class TransactionsListViewModel : BindableBase, IDataErrorInfo
    {
        private TransactionsService _transactionsService;
        private TransactionDetailsViewModel _currentTransaction;
        private WalletDetailsViewModel _wallet;

        private List<TransactionDetailsViewModel> _allTransactions;

        private int _from;
        private int _to;

        private bool _isEnabled;
        private bool _isIndeterminate;
        private Visibility _visibility;

        private bool _canApplyRange;
        private Visibility _rangeVisible;

        public ObservableCollection<TransactionDetailsViewModel> Transactions { get; set; }

        private decimal _initialTransactionSum;

        public TransactionDetailsViewModel CurrentTransaction
        {
            get => _currentTransaction;
            set
            {
                if (_currentTransaction != value)
                {
                    _currentTransaction = value;
                    if (_currentTransaction != null)
                    {
                        _initialTransactionSum = _currentTransaction.Sum;
                    }

                    RaisePropertyChanged();
                }
            }
        }

        public int From
        {
            get => _from;
            set
            {
                if (_from != value)
                {
                    _from = value;
                    CanApplyRange = this[nameof(From)] == string.Empty && this[nameof(To)] == string.Empty;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(To));
                    // ApplyRangeCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public int To
        {
            get => _to;
            set
            {
                if (_to != value)
                {
                    _to = value;
                    CanApplyRange = this[nameof(From)] == string.Empty && this[nameof(To)] == string.Empty;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(From));
                    // ApplyRangeCommand.RaiseCanExecuteChanged();
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

        public bool CanApplyRange
        {
            get => _canApplyRange;
            set
            {
                if (_canApplyRange != value)
                {
                    _canApplyRange = value;
                    RaisePropertyChanged();
                    ApplyRangeCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public Visibility RangeVisible
        {
            get => _rangeVisible;
            set
            {
                if (_rangeVisible != value)
                {
                    _rangeVisible = value;
                    RaisePropertyChanged();
                }
            }
        }

        public DelegateCommand ApplyRangeCommand { get; }
        public DelegateCommand AddTransactionCommand { get; }
        public DelegateCommand RemoveAllTransactionsCommand { get; }

        public TransactionsListViewModel(WalletDetailsViewModel wallet)
        {
            _isEnabled = true;
            _isIndeterminate = false;
            _visibility = Visibility.Hidden;

            _transactionsService = new TransactionsService(wallet.Guid);
            _allTransactions = new List<TransactionDetailsViewModel>();
            _wallet = wallet;
            Transactions = new ObservableCollection<TransactionDetailsViewModel>();

            ApplyRangeCommand = new DelegateCommand(ReloadTransactions, () => CanApplyRange);
            AddTransactionCommand = new DelegateCommand(AddTransaction);
            RemoveAllTransactionsCommand = new DelegateCommand(RemoveAllTransactions);

            List<Transaction> loadedTransactions = Task.Run(_transactionsService.GetTransactions).Result;
            loadedTransactions.Reverse();
            decimal allSum = 0;
            decimal incomes = 0;
            decimal expenses = 0;
            foreach (Transaction transaction in loadedTransactions)
            {
                _allTransactions.Add(new TransactionDetailsViewModel(this, transaction));
                allSum += transaction.Sum;
                DateTimeOffset now = DateTimeOffset.Now;
                if (transaction.Date >= new DateTimeOffset(now.Year, now.Month, 1, 0, 0, 0, now.Offset))
                {
                    if (transaction.Sum > 0)
                    {
                        incomes += transaction.Sum;
                    }
                    else if (transaction.Sum < 0)
                    {
                        expenses += transaction.Sum;
                    }
                } 
            }

            // To = _allTransactions.Count;
            To = Math.Max(1, _allTransactions.Count);
            From = Math.Max(1, To - 9);
            ReloadTransactions();

            _wallet.CurrentBalance = _wallet.InitialBalance + allSum;
            _wallet.IncomesThisMonth = incomes;
            _wallet.ExpensesThisMonth = expenses;
        }


        private async void AddTransaction()
        {
            IsEnabled = false;
            IsIndeterminate = true;
            Visibility = Visibility.Visible;

            Transaction newTransaction = new Transaction(Guid.NewGuid(), 0.0m, "UAH", new Category(1), "", DateTimeOffset.Now);

            await _transactionsService.AddOrUpdateTransaction(new SaveTransaction()
            {
                Guid = newTransaction.Guid,
                Sum = newTransaction.Sum,
                Currency = newTransaction.Currency,
                Category = newTransaction.Category,
                Description = newTransaction.Description,
                Date = newTransaction.Date
            });

            // Transactions.Insert(0, new TransactionDetailsViewModel(this, newTransaction, _transactionsService));
            _allTransactions.Insert(0, new TransactionDetailsViewModel(this, newTransaction));

            // To = _allTransactions.Count;
            To = Math.Max(1, _allTransactions.Count);
            From = Math.Max(1, To - 9);
            ReloadTransactions();

            Visibility = Visibility.Hidden;
            IsIndeterminate = false;
            IsEnabled = true;
        }

        public async void SaveTransaction()
        {
            IsEnabled = false;
            IsIndeterminate = true;
            Visibility = Visibility.Visible;

            await _transactionsService.AddOrUpdateTransaction(new SaveTransaction()
            {
                Guid = CurrentTransaction.Guid,
                Sum = CurrentTransaction.Sum,
                Currency = CurrentTransaction.Currency,
                Category = CurrentTransaction.Category,
                Description = CurrentTransaction.Description,
                Date = CurrentTransaction.Date
            });

            _wallet.CurrentBalance += CurrentTransaction.Sum - _initialTransactionSum;
            DateTimeOffset now = DateTimeOffset.Now;
            if (CurrentTransaction.Date >= new DateTimeOffset(now.Year, now.Month, 1, 0, 0, 0, now.Offset))
            {
                if (CurrentTransaction.Sum > 0)
                {
                    if (_initialTransactionSum < 0)
                    {
                        _wallet.IncomesThisMonth += CurrentTransaction.Sum;
                        _wallet.ExpensesThisMonth -= _initialTransactionSum;
                    }
                    else
                    {
                        _wallet.IncomesThisMonth += CurrentTransaction.Sum - _initialTransactionSum;
                    }
                }
                else if (CurrentTransaction.Sum < 0)
                {
                    if (_initialTransactionSum > 0)
                    {
                        _wallet.ExpensesThisMonth += CurrentTransaction.Sum;
                        _wallet.IncomesThisMonth -= _initialTransactionSum;
                    }
                    else
                    {
                        _wallet.ExpensesThisMonth += CurrentTransaction.Sum - _initialTransactionSum;
                    }
                }
            }
            // _initialTransactionSum = CurrentTransaction.Sum;
            SortTransactions();

            Visibility = Visibility.Hidden;
            IsIndeterminate = false;
            IsEnabled = true;
        }

        public async void RemoveTransaction()
        {
            IsEnabled = false;
            IsIndeterminate = true;
            Visibility = Visibility.Visible;

            if (await _transactionsService.RemoveTransaction(CurrentTransaction.Guid))
            {
                _wallet.CurrentBalance -= CurrentTransaction.Sum;
                DateTimeOffset now = DateTimeOffset.Now;
                if (CurrentTransaction.Date >= new DateTimeOffset(now.Year, now.Month, 1, 0, 0, 0, now.Offset))
                {
                    if (CurrentTransaction.Sum > 0)
                    {
                        _wallet.IncomesThisMonth -= _initialTransactionSum;
                    }
                    else if (CurrentTransaction.Sum < 0)
                    {
                        _wallet.ExpensesThisMonth -= _initialTransactionSum;
                    }
                }
                int delta = To - From;
                _allTransactions.Remove(CurrentTransaction);
                // To = Math.Min(To, _allTransactions.Count);
                To = Math.Min(To, Math.Max(1, _allTransactions.Count));
                From = Math.Max(1, To - delta);
                // CurrentTransaction = null;
                ReloadTransactions();
            }

            Visibility = Visibility.Hidden;
            IsIndeterminate = false;
            IsEnabled = true;
        }

        private async void RemoveAllTransactions()
        {
            IsEnabled = false;
            IsIndeterminate = true;
            Visibility = Visibility.Visible;

            if (await _transactionsService.RemoveTransaction(Guid.Empty))
            {
                Transactions.Clear();
            }
            CurrentTransaction = null;

            Visibility = Visibility.Hidden;
            IsIndeterminate = false;
            IsEnabled = true;
        }

        private void ReloadTransactions()
        {
            // IsEnabled = false;
            // IsIndeterminate = true;
            // Visibility = Visibility.Visible;

            CanApplyRange = false;

            Transactions.Clear();
            CurrentTransaction = null;

            if (_allTransactions.Count == 0)
            {
                RangeVisible = Visibility.Hidden;
            }
            else
            {
                RangeVisible = Visibility.Visible;
                for (int i = _allTransactions.Count - To; i < _allTransactions.Count - From + 1; ++i)
                {
                    Transactions.Add(_allTransactions[i]);
                }
            }

            // Visibility = Visibility.Hidden;
            // IsIndeterminate = false;
            // IsEnabled = true;
        }

        public void SortTransactions()
        {
            _allTransactions.Remove(CurrentTransaction);
            int i = 0;
            for (; i < _allTransactions.Count; ++i)
            {
                if (_allTransactions[i].Date < CurrentTransaction.Date)
                {
                    break;
                }
            }
            _allTransactions.Insert(i, CurrentTransaction);
            TransactionDetailsViewModel temp = CurrentTransaction;
            ReloadTransactions();
            CurrentTransaction = temp;
        }

        public string Error { get; }

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case "From":
                        if (From < 1)
                        {
                            // error = "From cannot be less than 1";
                            From = 1;
                        } 
                        else if (To < From)
                        {
                            error = "From cannot be greater than To";
                        }
                        else if (To - From > 9)
                        {
                            error = "Range cannot be more than 10";
                        }
                        break;
                    case "To":
                        if (To < 1)
                        {
                            // error = "To cannot be less than 1";
                            To = 1;
                        }
                        else if (To < From)
                        {
                            error = "To cannot be less than From";
                        }
                        else if (To > _allTransactions.Count)
                        {
                            // error = "To cannot be greater than number of all transactions";
                            To = Math.Max(1, _allTransactions.Count);
                        }
                        else if (To - From > 9)
                        {
                            error = "Range cannot be more than 10";
                        }
                        break;
                }

                return error;
            }
        }
    }
}
