using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TeamProject.Base;
using TeamProject.DTOs;
using TeamProject.Services;
using RelayCommand = TeamProject.Base.RelayCommand;

namespace TeamProject.ViewModels;

public class LoginViewModal:ViewModelBase
{
    private string _errorMessage = string.Empty;
     private string _password;
     private string _email;
     private UserDto? _userDto;
     private INavigationService _navigationService;
     private readonly IUserService _userService;
     private readonly IAuthenticationService _authenticationService;
     public RelayCommand NavigateToRegistration { get; set; }
     public AsyncRelayCommand LoginCommand { get; set; }
     public RelayCommand NavigateToImageEditor { get; set; }

     public LoginViewModal(INavigationService navService, IUserService userService,IAuthenticationService authService)
    {
        _authenticationService = authService;
        _navigationService = navService;
        _userService = userService;  
        LoginCommand = new AsyncRelayCommand(ExecuteLoginAsync,
            CanExecuteLogin);
        NavigateToRegistration= new RelayCommand(o => Navigation.NavigateTo<RegistrationViewModel>(),
            o => true);
        NavigateToImageEditor= new RelayCommand(o => Navigation.NavigateTo<ImageEditorViewModel>(),
            o => true);
    }

 

    private bool CanExecuteLogin()
    {
        return !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);

        

    }

    private async Task ExecuteLoginAsync()
    {
        try
        {
           
            var userDtoTemp = new UserDto
            {
                Email = Email,
                Password = Password
            };
            if (userDtoTemp.Email != _userDto?.Email)
            {
                
                _userDto  = await _userService.LoginUserAsync(userDtoTemp);
                if (_userDto == null)
                {
                    ErrorMessage = "UserNotFound";
                    return;
                }
                
            }

            if (_userDto.Password == Password)
            {
                _authenticationService.Login(_userDto);
                
                NavigateToImageEditor.Execute(null);


            }else { ErrorMessage = "WrongPassword"; }
            
           
            

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    


    private INavigationService Navigation
    {
        get=>_navigationService;
        set
        {
            _navigationService=value;
            OnPropertyChanged();
          
        }
    }
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }
    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
            LoginCommand.NotifyCanExecuteChanged();
        }
    }
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
            LoginCommand.NotifyCanExecuteChanged();
        }
    }
    public UserDto UserDto
    {
        get => _userDto;
        set
        {
            _userDto = value;
            OnPropertyChanged(nameof(UserDto));
        }
    }
    
}