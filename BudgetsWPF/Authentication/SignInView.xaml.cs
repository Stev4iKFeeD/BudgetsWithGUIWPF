using System.Windows;
using System.Windows.Controls;

namespace Budgets.GUI.WPF.Authentication
{
    /// <summary>
    /// Interaction logic for SignInView.xaml
    /// </summary>
    public partial class SignInView : UserControl
    {
       public SignInView()
        {
            InitializeComponent();
        }


        private void PbPassword_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            ((SignInViewModel) DataContext).Password = PbPassword.Password;
        }
    }
}
