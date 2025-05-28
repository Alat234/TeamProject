using System.Windows;
using System.Windows.Controls;
using TeamProject.ViewModels;

namespace TeamProject.Views;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();
        
    }
    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (sender is PasswordBox passwordBox)
        {
            var viewModel = DataContext as LoginViewModal;
            if (viewModel != null)
            {
                viewModel.Password = passwordBox.Password;
            }
        }
    }
}