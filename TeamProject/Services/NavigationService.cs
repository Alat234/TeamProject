using TeamProject.Base;
using TeamProject.Store;

namespace TeamProject.Services;

public interface INavigationService
{
    void NavigateTo<T>() where T : ViewModelBase;
}

public class NavigationService : ObservableObject, INavigationService
{
    private readonly Func<Type, ViewModelBase> _viewModelFactory;
    private readonly NavigationStore _navigationStore;

    public NavigationService(Func<Type, ViewModelBase> viewModelFactory, NavigationStore navigationStore)
    {
        _viewModelFactory = viewModelFactory;
        _navigationStore = navigationStore;
    }

    public void NavigateTo<T>() where T : ViewModelBase
    {
        
        ViewModelBase viewModel = _viewModelFactory.Invoke(typeof(T));
        if (viewModel != null)
        {
            
            _navigationStore.CurrentViewModel = viewModel;
        }
       
    }
}