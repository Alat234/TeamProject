using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using TeamProject.Base;
using TeamProject.DTOs;
using TeamProject.Models.Filters;
using TeamProject.Services;


namespace TeamProject.ViewModels;

public class ImageEditorViewModel:ViewModelBase
{
    private bool _isProgresSaved=true;
    private ImageDto? _imageDtoFromDb;
    private double _imageBlendValue;
    private NavigationBarViewModel _navigationBarViewModel;
    private BitmapSource _imageSource;
    private BitmapSource _editedImageSource;
    private Color _selectedColor;
    private  string _secretText ;
    private readonly IImageEditorService _imageEditorService;
    private bool _isImageLoadedManually;
    private bool _isImageHasSecretText=false;
    public RelayCommand AddNoiseCommand { get; set; }
    public RelayCommand SaveChangesCommand { get; set; }
    public RelayCommand UndoEditChangesCommand { get; set; }
    public RelayCommand AddSecretTextCommand { get; set; }
    public RelayCommand GetSecretTextCommend { get; set; }
    public RelayCommand ApplyFilterCommend { get; set; }
    public RelayCommand SaveImageOnPCCommand { get; set; }
 

    private readonly IAuthenticationService _authService;
    private readonly IImageService _imageServise;
    private BuildMethod _selectedBuildMethod;
    private ColorPalette _selectedColorPalette;

    
    public RelayCommand LoadImageCommand { get; set; }
    public ImageEditorViewModel( IImageEditorService imageEditorService,IAuthenticationService authService, 
        INavigationService navigationService, IImageService imageService,IUserService userService)
    {
      
        _imageServise = imageService;
        
        _authService = authService;
        _imageEditorService = imageEditorService;
        NavigationBarViewModel = new NavigationBarViewModel(navigationService, authService,userService);
        LoadImageCommand = new RelayCommand(execute: _ => LoadImage());
        UndoEditChangesCommand = new RelayCommand(execute: _ => UndoEditChanges(), canExecute: _ => ImageSource != EditedImageSource);
        SaveChangesCommand= new RelayCommand(execute: _ => SaveChanges(),canExecute:_=> _authService.CurrentUser!=null);
        AddSecretTextCommand = new RelayCommand(execute: _ => AddSecretText(), canExecute:_=> _secretText!=null);
        GetSecretTextCommend= new RelayCommand(execute: _ => GetSecretText(),canExecute:_=> _editedImageSource!=null);
        AddNoiseCommand = new RelayCommand(execute: _ => AddNoise());
        SaveImageOnPCCommand= new RelayCommand(execute: _ => SaveImageOnPC(),canExecute:_=> _editedImageSource!=null);
        ApplyFilterCommend = new RelayCommand(execute: _ => ApplyFilter());
        SetImage();
        ImageBlendValue = 0;
    }

    private void SaveImageOnPC()
    {
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "BMP Files (*.bmp)|*.bmp|PNG Files (*.png)|*.png|All Files (*.*)|*.*",
            Title = "Save Image",
            FileName = "EditedImage.bmp" 
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            try
            {
                BitmapEncoder encoder;
                string extension = Path.GetExtension(saveFileDialog.FileName).ToLower();

                // Вибір кодека залежно від розширення
                switch (extension)
                {
                    case ".png":
                        encoder = new PngBitmapEncoder();
                        break;
                    case ".bmp":
                        encoder = new BmpBitmapEncoder();
                        break;
                    default:
                        MessageBox.Show("Unsupported file format. Please use .bmp or .png.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                }

                encoder.Frames.Add(BitmapFrame.Create(EditedImageSource));

                using (var fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    encoder.Save(fileStream);
                }

                MessageBox.Show("Image saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save the image. Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    
    }

    private void SetImage()
    {
        _imageDtoFromDb = _imageServise.GetImageFromStore();
        if (_imageDtoFromDb != null && !_isImageLoadedManually)
        {
            ImageSource = _imageDtoFromDb.ImageSource;
            EditedImageSource = _imageDtoFromDb.ImageSource;
        }
        else if (_authService.CurrentUser != null && !_isImageLoadedManually)
        {
            _imageDtoFromDb = new ImageDto
            {
                ImageSource = null,
                UserId = _authService.CurrentUser.Id
            };
            _imageServise.SetImageInStore(_imageDtoFromDb);
        }
        else if (!_isImageLoadedManually)
        {
            _imageDtoFromDb = new ImageDto
            {
                ImageSource = null
            };
            _imageServise.SetImageInStore(_imageDtoFromDb);
        }
        
          
        
      
    }

    private void ApplyFilter()
    {
        if ( EditedImageSource == null)
            return;

        EditedImageSource = _imageEditorService.ApplyFilter(EditedImageSource,SelectedBuildMethod , SelectedColorPalette);
        _isProgresSaved = false;
        ImageBlendValue = 1;
        _imageDtoFromDb.ImageSource = EditedImageSource;
        _imageServise.SetImageInStore(_imageDtoFromDb);
    }

    private void AddSecretText()
    {
        
        if (ImageSource == null)
            return;
      
        EditedImageSource = _imageEditorService.AddSecretText(_editedImageSource, _secretText);
        _isProgresSaved = false;
        _imageDtoFromDb.ImageSource = EditedImageSource;
        _imageServise.SetImageInStore(_imageDtoFromDb);
        if (_secretText != null)
        {
            _imageDtoFromDb.HasSecretText = true;
            _isImageHasSecretText=true;
            
        }
        
        
        ImageBlendValue = 1;
    }

    private void GetSecretText()
    {
        if (ImageSource == null)
            return;
        SecretText = _imageEditorService.GetSecretText(_editedImageSource);
        if (_secretText != null)
        {
            _imageDtoFromDb.HasSecretText = true;
            _isImageHasSecretText=true;
            
        }
        
    }


    public void SaveChanges()
    {
        if (_imageDtoFromDb.Id == 0)
        {
            var imageDto = new ImageDto()
            {
                ImageSource = _editedImageSource,
                UserId = _authService.CurrentUser.Id,
                HasSecretText = _isImageHasSecretText
                

            };
            _imageServise.AddImageAsync(imageDto);
        }
        else
        {
            _imageDtoFromDb.ImageSource = _editedImageSource;
            _imageServise.UpdateImageAsync(_imageDtoFromDb);
            
        }
        _isProgresSaved = true;


    }
    public BuildMethod SelectedBuildMethod
    {
        get => _selectedBuildMethod;
        set
        {
            _selectedBuildMethod = value;
            OnPropertyChanged();
        }
    }

    public ColorPalette SelectedColorPalette
    {
        get => _selectedColorPalette;
        set
        {
            _selectedColorPalette = value;
            OnPropertyChanged();
        }
    }


    public string SecretText
    {
        get => _secretText;
        set
        {
            _secretText = value;
            OnPropertyChanged();
        }
    }

    public double ImageBlendValue
    {
        get => _imageBlendValue;
        set
        {
            _imageBlendValue = value;
            OnPropertyChanged();
        }
    }
    
    public NavigationBarViewModel NavigationBarViewModel
    {
        get => _navigationBarViewModel;
        set
        {
            _navigationBarViewModel = value;
            OnPropertyChanged();
        }
    }
    public Color SelectedColor
    {
        get => _selectedColor;
        set
        {
            _selectedColor = value;
            OnPropertyChanged();
        }
    }
    public BitmapSource EditedImageSource
    {
        get => _editedImageSource;
        set
        {
            _editedImageSource = value;
            OnPropertyChanged();
        }
    }
    public BitmapSource ImageSource
    {
        get => _imageSource;
        set
        {
            _imageSource = value;
            OnPropertyChanged();
        }
    }
    private void LoadImage()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "BMP Files (*.bmp)|*.bmp|All Files (*.*)|*.*", 
            Title = "Select a BMP Image"
        };

       if (openFileDialog.ShowDialog() == true)
       {
           string filePath = openFileDialog.FileName;

  
           if (!Path.GetExtension(filePath).Equals(".bmp", StringComparison.OrdinalIgnoreCase))
           {
               MessageBox.Show(
                   "Please select a valid BMP file. The selected file does not have a .bmp extension.",
                   "Invalid File Format",
                   MessageBoxButton.OK,
                   MessageBoxImage.Error);
               return;
           }

           try
           {
               
               var bitmapImage = new BitmapImage();
               bitmapImage.BeginInit();
               bitmapImage.UriSource = new Uri(filePath);
               bitmapImage.EndInit();

              
               ImageSource = bitmapImage;
               EditedImageSource = bitmapImage;
               _imageDtoFromDb.ImageSource =EditedImageSource ;
               _imageServise.SetImageInStore(_imageDtoFromDb);
               _isImageLoadedManually = true;
           }
           catch (Exception ex)
           {
               
               MessageBox.Show(
                   $"Failed to load the image. It may not be a valid BMP file.\nError: {ex.Message}",
                   "Error Loading Image",
                   MessageBoxButton.OK,
                   MessageBoxImage.Error);
           }
       }
    }

    private void AddNoise()
    {
        if (ImageSource == null)
            return;
        
        EditedImageSource=_imageEditorService.AddNoise(_editedImageSource);
        ImageBlendValue = 1;
        _isProgresSaved = false;
        _imageDtoFromDb.ImageSource =EditedImageSource ;
        _imageServise.SetImageInStore(_imageDtoFromDb);
    }

    private void UndoEditChanges()
    {
        if (ImageSource != null)
        {
            EditedImageSource = ImageSource;
            ImageBlendValue = 0;
        }
        _imageDtoFromDb.ImageSource =EditedImageSource ;
        _imageServise.SetImageInStore(_imageDtoFromDb);
    }

    public bool IsProgressSaved
    {
        get => _isProgresSaved;
        set
        {
            _isProgresSaved = value;
            OnPropertyChanged();
        }
    }
    
}