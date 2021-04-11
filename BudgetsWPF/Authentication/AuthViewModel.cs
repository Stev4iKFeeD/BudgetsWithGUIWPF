using System;
using Budgets.GUI.WPF.Navigation;

namespace Budgets.GUI.WPF.Authentication
{
    public class AuthViewModel : NavigationBase<AuthNavigatableType>, INavigatable<MainNavigatableType>
    {
        private Action _signInSuccess;
        
        public AuthViewModel(Action signInSuccess)
        {
            _signInSuccess = signInSuccess;
            Navigate(AuthNavigatableType.SignIn);
        }
        
        protected override INavigatable<AuthNavigatableType> CreateViewModel(AuthNavigatableType type)
        {
            switch (type)
            {
                case AuthNavigatableType.SignIn:
                    return new SignInViewModel(() => Navigate(AuthNavigatableType.SignUp), _signInSuccess);
                case AuthNavigatableType.SignUp:
                    return new SignUpViewModel(() => Navigate(AuthNavigatableType.SignIn));
                default:
                    return null;
            }
        }

        public MainNavigatableType Type => MainNavigatableType.Auth;
        public void ClearSensitiveData()
        {
            CurrentViewModel.ClearSensitiveData();
        }
    }
}
