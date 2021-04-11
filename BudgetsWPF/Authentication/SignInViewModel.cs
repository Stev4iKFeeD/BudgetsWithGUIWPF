using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Budgets.BusinessLayer.User;
using Budgets.GUI.WPF.Navigation;
using Budgets.Services;
using Prism.Commands;

namespace Budgets.GUI.WPF.Authentication
{
    public class SignInViewModel : INotifyPropertyChanged, INavigatable<AuthNavigatableType>
    {
        private AuthUser _authUser;

        private bool _isEnabled;
        private bool _isIndeterminate;
        private Visibility _visibility;

        public string Login
        {
            get => _authUser.Login;
            set
            {
                if (_authUser.Login != value)
                {
                    _authUser.Login = value;
                    OnPropertyChanged();
                    SignInCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Password
        {
            get => _authUser.Password;
            set
            {
                if (_authUser.Password != value)
                {
                    _authUser.Password = value;
                    OnPropertyChanged();
                    SignInCommand.RaiseCanExecuteChanged();
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

        public DelegateCommand SignInCommand { get; }
        public DelegateCommand GoToSignUpCommand { get; }
        public DelegateCommand CloseCommand { get; }
        public DelegateCommand GoToWalletsCommand { get; }


        public AuthNavigatableType Type => AuthNavigatableType.SignIn;


        public SignInViewModel(Action gotoSignUp, Action gotoWallets)
        {
            _authUser = new AuthUser();
            _isEnabled = true;
            _isIndeterminate = false;
            _visibility = Visibility.Hidden;

            SignInCommand = new DelegateCommand(SignIn, IsSignInEnabled);
            GoToSignUpCommand = new DelegateCommand(gotoSignUp);
            CloseCommand = new DelegateCommand(() => Environment.Exit(0));

            GoToWalletsCommand = new DelegateCommand(gotoWallets);
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private async void SignIn()
        {
            IsEnabled = false;
            IsIndeterminate = true;
            Visibility = Visibility.Visible;

            AuthService authService = new AuthService();
            try
            {
                User user = await authService.Authenticate(_authUser);
                CurrentUserInfo.Guid = user.Guid;

                MessageBox.Show($"Sign In is successful for user {user.FirstName} {user.LastName}", "Sign In");
                await Task.Run(() => GoToWalletsCommand.Execute());
            }
            catch (Exception e)
            {
                MessageBox.Show("Sign In failed: " + e.Message, "Sign In");
            }
            finally
            {
                Visibility = Visibility.Hidden;
                IsIndeterminate = false;
                IsEnabled = true;
            }
        }

        private bool IsSignInEnabled()
        {
            return !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password);
        }
        

        public void ClearSensitiveData()
        {
            Password = "";
        }
    }
}
