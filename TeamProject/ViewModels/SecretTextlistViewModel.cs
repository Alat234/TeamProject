// TeamProject.ViewModels/SecretTextlistViewModel.cs
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input; // Для ICommand
using TeamProject.Base;
// Припустимо, тут у вас є RelayCommand або AsyncRelayCommand
using TeamProject.DTOs;
using TeamProject.Services;

namespace TeamProject.ViewModels;

public class SecretTextlistViewModel : ViewModelBase
{
    private NavigationBarViewModel _navigationBarViewModel;
    private readonly IImageService _imageService;
    private readonly IAuthenticationService _authService;
    private readonly IImageEditorService _imageEditorService;
    private ObservableCollection<ImageDto> _userImagesWithSecrettext;

    private string _currentSecretText; // Поле для зберігання видобутого секретного тексту
    private bool _isSecretTextVisible; // Поле для керування видимістю текстового блоку
    private readonly INavigationService _navigationService;

    public NavigationBarViewModel NavigationBarViewModel
    {
        get => _navigationBarViewModel;
        set
        {
            _navigationBarViewModel = value;
            OnPropertyChanged();
        }
    }
    public ObservableCollection<ImageDto> UserImages
    {
        get => _userImagesWithSecrettext;
        set
        {
            _userImagesWithSecrettext = value;
            OnPropertyChanged();    
        }
    }

    public string CurrentSecretText 
    {
        get => _currentSecretText;
        set
        {
            _currentSecretText = value;
            OnPropertyChanged();
        }
    }

    public bool IsSecretTextVisible 
    {
        get => _isSecretTextVisible;
        set
        {
            _isSecretTextVisible = value;
            OnPropertyChanged();
        }
    }

    // Команди
    public ICommand EditImageCommand { get; }
    public ICommand DeleteImageCommand { get; }
    public ICommand ShowSecretTextCommand { get; } 

    public SecretTextlistViewModel(IImageEditorService imageEditorService,IAuthenticationService authService, 
        INavigationService navigationService, IImageService imageService,IUserService userService)
    {
        NavigationBarViewModel = new NavigationBarViewModel(navigationService, authService,userService);
        _navigationService = navigationService;
        _imageService = imageService;
        UserImages = new ObservableCollection<ImageDto>();
        _authService = authService;
        _imageEditorService = imageEditorService;

     
       
        ShowSecretTextCommand = new RelayCommand(ExecuteShowSecretTextCommand); 
        EditImageCommand = new RelayCommand(execute: async parameter => await EditImage(parameter), canExecute: _ => true);
        DeleteImageCommand =
            new RelayCommand(execute: async parameter => await DeleteImage(parameter), canExecute: _ => true);

        LoadImagesAsync();
    }

    private async Task DeleteImage(object parameter)
    {
        if (parameter is ImageDto image)
        {
            await  _imageService.DeleteImageById(image.Id);
            UserImages.Remove(image);
        }
    }

    private async Task EditImage(object parameter)
    {
        if (parameter is ImageDto image)
        {
            _imageService.SetImageInStore(image);
            _navigationService.NavigateTo<ImageEditorViewModel>();
        }
    }

    private async Task LoadImagesAsync()
    {
        var userId = _authService.CurrentUser?.Id;
        if (userId == null || userId <= 0) return;

        var images = await _imageService.GetListOfImageWithSecretText(userId.Value);
        UserImages.Clear();
        foreach (var image in images)
        {
            string extractedText = _imageEditorService.GetSecretText(image.ImageSource); 
            

            image.SecretText = string.IsNullOrEmpty(extractedText) ? "Не знайдено": extractedText;
            UserImages.Add(image);
        }
    }


   
    

    private  void ExecuteShowSecretTextCommand(object parameter)
    {
        if (parameter is ImageDto imageDto)
        {
            
            IsSecretTextVisible = false;
            CurrentSecretText = "Завантаження..."; 

            try
            {
                // Видобуваємо секретний текст для конкретного зображення
                string extractedText =  _imageEditorService.GetSecretText(imageDto.ImageSource);

                if (!string.IsNullOrEmpty(extractedText))
                {
                    CurrentSecretText = extractedText;
                    IsSecretTextVisible = true; // Робимо текстовий блок видимим
                }
                else
                {
                    CurrentSecretText = "Секретний текст не знайдено.";
                    IsSecretTextVisible = true;
                }
            }
            catch (Exception ex)
            {
                CurrentSecretText = $"Помилка при видобуванні тексту: {ex.Message}";
                IsSecretTextVisible = true;
            }
        }
    }
}