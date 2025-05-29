using System.Windows;
using System.Windows.Controls;
using TeamProject.ViewModels;

namespace TeamProject.Views;

public partial class NavigationBar : UserControl
{
    public NavigationBar()
    {
        InitializeComponent();
    }
    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs routedEventArgs)
    {
        if (sender is PasswordBox passwordBox)
        {
            var viewModel = DataContext as NavigationBarViewModel;
            if (viewModel != null)
            {
                viewModel.EnteredPassword = passwordBox.Password;
            }
        }
    }
}