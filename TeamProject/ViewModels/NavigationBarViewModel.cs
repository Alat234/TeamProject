using System.Windows;
using System.Windows.Controls;
using AutoMapper;
using TeamProject.Base;
using TeamProject.DTOs;
using TeamProject.Services;
using TeamProject.Views;

namespace TeamProject.ViewModels;

public class NavigationBarViewModel : ViewModelBase
{
    private UserDto _userDto;
    private INavigationService _navService;
    private readonly IAuthenticationService _authService;
    private string _userName;
    private string _userEmail;
    private bool _isPopupOpen;
    private bool _isPasswordVerified=false;
    private string _enteredPassword;
    private readonly IUserService _userService;

    public RelayCommand LogoutCommand { get; set; }
    public RelayCommand ShowUserInfoCommand { get; set; }
    public RelayCommand NavigateToMyImagesCommand { get; set; }
    public RelayCommand NavigateToImageEditorCommand { get; set; }
    public RelayCommand NavigateToCreatorInfo { get; set; }
    public RelayCommand ShowInfoAboutProgramCommand { get; set; }
    public RelayCommand ShowInfoAboutCreatorsCommand { get; set; }
    public RelayCommand ShowInstructionCommand { get; set; }
    public RelayCommand NavigateSecretTextList { get; set; }
    public NavigationBarViewModel(INavigationService navService, IAuthenticationService authService,IUserService userService )
    {
        _userService = userService;
        _navService = navService ?? throw new ArgumentNullException(nameof(navService));
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        GetUserData();

      
        ShowUserInfoCommand = new RelayCommand(execute: _ => ShowUserInfo());
        LogoutCommand = new RelayCommand(execute: _ => Logout());
        NavigateToCreatorInfo=  new RelayCommand(execute: _ =>Navigation.NavigateTo<MyImageViewModel>() );
        
        NavigateToMyImagesCommand = new RelayCommand(execute: _ =>Navigation.NavigateTo<MyImageViewModel>() );
        NavigateToImageEditorCommand = new RelayCommand(execute: _ =>Navigation.NavigateTo<ImageEditorViewModel>() );
        ShowInfoAboutProgramCommand = new RelayCommand(execute: _ =>ShowInfoAboutProgram() );
        ShowInfoAboutCreatorsCommand=new  RelayCommand(execute: _ =>ShowInfoAboutCreators() );
        ShowInstructionCommand=new RelayCommand(execute: _ =>ShowInstruction() );
        NavigateSecretTextList= new RelayCommand(execute: _ =>Navigation.NavigateTo<SecretTextlistViewModel>() ,_=> CanShowSecretTextList());
        
    }

    private bool CanShowSecretTextList()
    {
       
        if (VerifyPassword())
        {
            return true;
        }

        return false;
    }


    public String EnteredPassword
    {
        get => _enteredPassword;
        set
        {
            _enteredPassword = value;
            OnPropertyChanged();
        }
    }
    private bool VerifyPassword()
    {
        if (_authService.CurrentUser != null && _enteredPassword == _userDto.Password)
        {
            return true;
        }
        
        return false;
        
    }

    private void ShowInstruction()
    {
        var instructionWindow = new UserInstructionWindow();
        instructionWindow.Show();
    }


    private void ShowInfoAboutCreators()
    {
        var aboutCreatorsWindow = new AboutCreatorsWindow();
        aboutCreatorsWindow.Show();
    }


    private void ShowInfoAboutProgram()
    {
        var aboutProgramWindow = new AboutProgram();
        aboutProgramWindow.Show();
    }

    

    private INavigationService Navigation
    {
        get=> _navService;
        set
        {
            _navService=value;
            OnPropertyChanged();
        }
    }
    public void RefreshUserData()
    {
        GetUserData();
        
    }

    public UserDto UserDto
    {
        get => _userDto;
        set
        {
            _userDto = value;
            OnPropertyChanged(nameof(UserDto));
            UpdateUserName(); 
        }
    }

    public string UserName
    {
        get => _userName;
        set
        {
            _userName = value;
            OnPropertyChanged(nameof(UserName));
        }
    }
    public string UserEmail
    {
        get => _userEmail;
        set
        {
            _userEmail = value;
            OnPropertyChanged(nameof(UserEmail));
        }
    }

    public bool IsPopupOpen
    {
        get => _isPopupOpen;
        set
        {
            _isPopupOpen = value;
            OnPropertyChanged(nameof(IsPopupOpen));
        }
    }
   

    public RelayCommand NavigateToLoginCommand { get; set; }
    

    private async Task GetUserData()
    {
        var tempuser=_authService.CurrentUser;
        if (tempuser != null)
        {
            var userDto = await _userService.GetUserById(tempuser.Id);
            UserDto = userDto;
          
        }
        
    }

    private void UpdateUserName()
    {
        UserName = _userDto?.Name ?? "Guest";
    }
    private void UpdateUserInfo()
    {
        UserName = _userDto?.Name ?? "Guest";
        UserEmail = _userDto?.Email ?? "No email";
    }
    private void ShowUserInfo()
    {
        
            UpdateUserInfo();
            IsPopupOpen = true;
        
    }
    private  void Logout()
    {
       _authService.Logout();
        GetUserData();
        IsPopupOpen = false; 
        _navService.NavigateTo<LoginViewModal>();
    }

    
}