    using TeamProject.Base;
    using TeamProject.Services;
    using TeamProject.Store;

    namespace TeamProject.ViewModels;

    public class MainViewModal : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly NavigationStore _navigationStore;
        private readonly IAuthenticationService _authService; // Додаємо IAuthenticationService

        public RelayCommand NavigateToLogin { get; set; }
        public RelayCommand NavigateToImageEditor { get; set; }
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public MainViewModal(INavigationService navigationService, NavigationStore navigationStore, IAuthenticationService authService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _navigationStore = navigationStore ?? throw new ArgumentNullException(nameof(navigationStore));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));

            NavigateToImageEditor = new RelayCommand(execute: _ => { _navigationService.NavigateTo<ImageEditorViewModel>(); }, canExecute: _ => true);
            NavigateToLogin = new RelayCommand(execute: _ => { _navigationService.NavigateTo<LoginViewModal>(); }, canExecute: _ => true);

            _navigationStore.PropertyChanged += (_, _) => OnPropertyChanged(nameof(CurrentViewModel));

         
            InitializeNavigation();
        }

        private void InitializeNavigation()
        {
            if (_authService.IsLoggedIn)
            {
                _navigationService.NavigateTo<ImageEditorViewModel>();
            }
            else
            {
                _navigationService.NavigateTo<LoginViewModal>();
            }
        }
    }