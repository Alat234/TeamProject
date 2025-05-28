using System.Text.RegularExpressions;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using TeamProject.Base;
using TeamProject.DTOs;
using TeamProject.Services;
using RelayCommand = TeamProject.Base.RelayCommand;

namespace TeamProject.ViewModels;

public class RegistrationViewModel:ViewModelBase
{
     
    private string _email = string.Empty;
    private string _userName = string.Empty;
    private string _password = string.Empty;
    private string _repeatedPassword = string.Empty;
    private string _errorMessage = string.Empty;
    private INavigationService _navservise;
    private readonly IUserService _userService;
    public  AsyncRelayCommand RegisterCommand { get; set; }
    public RelayCommand NavigateToLogin { get; set; }
    public RelayCommand NavigateToImageEditor { get; set; }


   
    public RegistrationViewModel(INavigationService navService, IUserService userService)
    {
        _userService=userService;
        _navservise=navService;
        RegisterCommand = new AsyncRelayCommand(ExecuteRegisterAsync, CanExecuteRegister);
        NavigateToLogin= new RelayCommand(o=>Navigation.NavigateTo<LoginViewModal>(),o=>true);
        NavigateToImageEditor = new RelayCommand(o => Navigation.NavigateTo<ImageEditorViewModel>(), o => true);
      
    }

   
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            
        }
    }
    

    private INavigationService Navigation
    {
        get=> _navservise;
        set
        {
            _navservise=value;
            OnPropertyChanged();
        }
    }
    public string Email
    {
        get => _email;
        set {

            if (value == _email) return; 
            _email = value;
            OnPropertyChanged();
            if (ValidEmail(value)) 
            {
                RegisterCommand.NotifyCanExecuteChanged();
            }
            
            
        }
    }
    public string Username
    {
        get => _userName;
        set
        {
            _userName = value;
            OnPropertyChanged();
            RegisterCommand.NotifyCanExecuteChanged();
        }

    }
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
            RegisterCommand.NotifyCanExecuteChanged();
        } 
    }
    public string RepeatedPassword
    {
        get => _repeatedPassword;

        set
        {
            _repeatedPassword = value;
            OnPropertyChanged();
            RegisterCommand.NotifyCanExecuteChanged();
        }
    }

    private bool  IsPasswordMatch()
    {
        return Password == RepeatedPassword;
    }
    private bool ValidEmail(string email)
    {
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!string.IsNullOrEmpty(email) && !Regex.IsMatch(email, pattern))
        {
            ErrorMessage = "Invalid email format.";
            return false;
        }
        return true;

    }
    private bool CanExecuteRegister()
    {
        return !string.IsNullOrEmpty(Email) &&
               !string.IsNullOrEmpty(Username) &&
               !string.IsNullOrEmpty(Password) &&
               !string.IsNullOrEmpty(RepeatedPassword) &&
               ValidEmail(Email);
    }

    private async Task ExecuteRegisterAsync()
    {
        if (!IsPasswordMatch())
        {
            ErrorMessage = "Passwords do not match.";
            return;
        }
        try
        {
            var userDto = new UserDto
            {
                Name = Username,
                Email = Email,
                Password = Password
            };

            var resultDto = await _userService.RegisterUserAsync(userDto);
            Email = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
            RepeatedPassword = string.Empty;
            NavigateToLogin.Execute(null);


        }
        catch (ArgumentException ex)
        {
            ErrorMessage = ex.Message; 
            OnPropertyChanged(nameof(ErrorMessage));
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Unexpected error during registration: {ex.Message}";
            OnPropertyChanged(nameof(ErrorMessage));
        }
    }
    
}