using TeamProject.Base;
using TeamProject.DTOs;

namespace TeamProject.Store;
public class ImageStore : ObservableObject
{
    private ImageDto? _currentImage;

    public ImageDto? CurrentImage
    {
        get => _currentImage;
        set
        {
            _currentImage = value;
            OnPropertyChanged();
        }
    }

    public ImageStore()
    {
        _currentImage = null;
    }

    public void SetImage(ImageDto? imageDto)
    {
        CurrentImage = imageDto;
    }

    public void ClearImage()
    {
        CurrentImage = null;
    }
}