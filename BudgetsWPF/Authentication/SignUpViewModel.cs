using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Windows;
using Budgets.BusinessLayer.User;
using Budgets.GUI.WPF.Navigation;
using Budgets.Services;
using Prism.Commands;

namespace Budgets.GUI.WPF.Authentication
{
    public class SignUpViewModel : INotifyPropertyChanged, INavigatable<AuthNavigatableType>, IDataErrorInfo
    {
        private RegUser _regUser;

        private bool _isEnabled;
        private bool _isIndeterminate;
        private Visibility _visibility;


        public string Login
        {
            get => _regUser.Login;
            set
            {
                if (_regUser.Login != value)
                {
                    _regUser.Login = value;
                    OnPropertyChanged();
                    SignUpCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Password
        {
            get => _regUser.Password;
            set
            {
                if (_regUser.Password != value)
                {
                    _regUser.Password = value;
                    OnPropertyChanged();
                    SignUpCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string FirstName
        {
            get => _regUser.FirstName;
            set
            {
                if (_regUser.FirstName != value)
                {
                    _regUser.FirstName = value;
                    OnPropertyChanged();
                    SignUpCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string LastName
        {
            get => _regUser.LastName;
            set
            {
                if (_regUser.LastName != value)
                {
                    _regUser.LastName = value;
                    OnPropertyChanged();
                    SignUpCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Email
        {
            get => _regUser.Email;
            set
            {
                if (_regUser.Email != value)
                {
                    _regUser.Email = value;
                    OnPropertyChanged();
                    SignUpCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnPropertyChanged();
                }
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                }
            }
        }

        public DelegateCommand SignUpCommand { get; }
        public DelegateCommand GoToSignInCommand { get; }
        public DelegateCommand CloseCommand { get; }


        public AuthNavigatableType Type => AuthNavigatableType.SignUp;


        public SignUpViewModel(Action gotoSignIn)
        {
            _regUser = new RegUser();
            _isEnabled = true;
            _isIndeterminate = false;
            _visibility = Visibility.Hidden;

            SignUpCommand = new DelegateCommand(SignUp, IsSignUpEnabled);
            GoToSignInCommand = new DelegateCommand(gotoSignIn);
            CloseCommand = new DelegateCommand(() => Environment.Exit(0));
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private async void SignUp()
        {
            IsEnabled = false;
            IsIndeterminate = true;
            Visibility = Visibility.Visible;

            AuthService authService = new AuthService();
            try
            {
                await authService.Register(_regUser);

                MessageBox.Show("User successfully registered. Please Sign In", "Sign Up");
                GoToSignInCommand.Execute();
            }
            catch (Exception e)
            {
                MessageBox.Show("Sign Up failed: " + e.Message, "Sign Up");
            }
            finally
            {
                Visibility = Visibility.Hidden;
                IsIndeterminate = false;
                IsEnabled = true;
            }
        }

        private bool IsSignUpEnabled()
        {
            return !string.IsNullOrWhiteSpace(Login)
                   && !string.IsNullOrWhiteSpace(Password)
                   && !string.IsNullOrWhiteSpace(FirstName)
                   && !string.IsNullOrWhiteSpace(LastName)
                   && this[nameof(Email)] == string.Empty;
        }


        public void ClearSensitiveData()
        {
            Login = "";
            Password = "";
            FirstName = "";
            LastName = "";
            Email = "";
        }

        public string Error { get; }

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case "Login":
                        if (string.IsNullOrWhiteSpace(Login))
                        {
                            error = "Login cannot be empty";
                        }
                        break;
                    case "FirstName":
                        if (string.IsNullOrWhiteSpace(FirstName))
                        {
                            error = "First Name cannot be empty";
                        }
                        break;
                    case "LastName":
                        if (string.IsNullOrWhiteSpace(LastName))
                        {
                            error = "Last Name cannot be empty";
                        }
                        break;
                    case "Email":
                        if (!new EmailAddressAttribute().IsValid(Email))
                        {
                            error = "Email is invalid";
                        }
                        break;
                }

                return error;
            }
        }
    }
}
