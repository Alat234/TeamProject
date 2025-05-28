using System.Windows.Controls;
using System.Windows;
using TeamProject.ViewModels;

namespace TeamProject.Views;

public partial class RegistrationView : UserControl
{
    public RegistrationView()
    {
        InitializeComponent();
    }
    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (sender is PasswordBox passwordBox)
        {
            var viewModel = DataContext as RegistrationViewModel;
            if (viewModel != null)
            {
                viewModel.Password = passwordBox.Password;
            }
        }
    }
    private void RepeatedPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (sender is PasswordBox passwordBox)
        {
            var viewModel = DataContext as RegistrationViewModel;
            if (viewModel != null)
            {
                viewModel.RepeatedPassword = passwordBox.Password;
            }
        }
    }
}