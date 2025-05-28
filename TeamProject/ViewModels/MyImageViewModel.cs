using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.Messaging;
using TeamProject.Base;
using TeamProject.DTOs;
using TeamProject.Models;
using TeamProject.Repositorie;
using TeamProject.Services;

namespace TeamProject.ViewModels;

public class MyImageViewModel : ViewModelBase
{
  
    
        private readonly IAuthenticationService _authService;
        private readonly INavigationService _navigationService;
        private NavigationBarViewModel _navigationBarViewModel;
        private ObservableCollection<ImageDto> _userImages;
        private readonly IImageService _imageService;
       

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
            get => _userImages;
            set
            {
                _userImages = value;
                OnPropertyChanged();
            }
        }

        public ICommand EditImageCommand { get; }
        public ICommand DeleteImageCommand { get; }

        public MyImageViewModel(IAuthenticationService authService, INavigationService navigationService, IImageService imageService)
        {
           
            _authService = authService;
            _navigationService = navigationService;
            _imageService =imageService;

            NavigationBarViewModel = new NavigationBarViewModel(navigationService, authService);
            UserImages = new ObservableCollection<ImageDto>();

            EditImageCommand = new RelayCommand(execute: async parameter => await EditImage(parameter), canExecute: _ => true);
            DeleteImageCommand = new RelayCommand(execute: async parameter => await DeleteImage(parameter), canExecute: _ => true);

            LoadImagesAsync();
        }

        private async Task LoadImagesAsync()
        {
            var userId = _authService.CurrentUser?.Id;
            if (userId == null || userId <= 0) return;

            var images = await _imageService.GetListOfImageByUserId(userId.Value);
            UserImages.Clear();
            foreach (var image in images)
            {
                
                UserImages.Add(image);
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

        private async Task DeleteImage(object parameter)
        {
            if (parameter is ImageDto image)
            {
              await  _imageService.DeleteImageById(image.Id);
              UserImages.Remove(image);
            }
        }

        
    }
    
