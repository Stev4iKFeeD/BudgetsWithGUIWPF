using Budgets.GUI.WPF.Authentication;
using Budgets.GUI.WPF.Navigation;
using Budgets.GUI.WPF.Wallets;

namespace Budgets.GUI.WPF
{
    public class MainViewModel : NavigationBase<MainNavigatableType>
    {
        public MainViewModel()
        {
            Navigate(MainNavigatableType.Auth);
        }
        
        protected override INavigatable<MainNavigatableType> CreateViewModel(MainNavigatableType type)
        {
            switch (type)
            {
                case MainNavigatableType.Auth:
                    return new AuthViewModel(() => Navigate(MainNavigatableType.Wallets));
                case MainNavigatableType.Wallets:
                    return new WalletsViewModel(() => Navigate(MainNavigatableType.Auth));
                default:
                    return null;
            }
        }
    }
}
