using System.ComponentModel;
using TeamProject.Base;

namespace TeamProject.Store;

public class NavigationStore : INotifyPropertyChanged
{
    private ViewModelBase _currentViewModel;

    public event PropertyChangedEventHandler PropertyChanged;

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}