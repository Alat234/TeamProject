using TeamProject.Commends.ImageDbCommand;
using TeamProject.DTOs;
using TeamProject.Store;

namespace TeamProject.Services;

public interface IImageService
{
    Task<ImageDto?> AddImageAsync(ImageDto imageDto);
    Task<ImageDto?> GetImageAsync(int id);
    Task<List<ImageDto>> GetListOfImageByUserId(int id);
    Task DeleteImageById(int id);

    Task<ImageDto?> UpdateImageAsync(ImageDto imageDto);
    
    
    void SetImageInStore(ImageDto imageDto);
    ImageDto? GetImageFromStore();
    Task<List<ImageDto>> GetListOfImageWithSecretText(int id);
    

}

public class ImageService : IImageService
{
    private readonly ImageStore ImageStore;
    private readonly IAddImageAsyn _addImageAsyn;
    private readonly IGetImgeByIdCommand _getImgeByIdCommand;
    private readonly IGetListOfImageByUserIdCommend _getListOfImageByUserIdCommend;
    private readonly IDeleteImageById _deleteImageById;
    private readonly IUpdateImageAsync _updateImageAsync;
    private readonly IGetImagesWithSecretTextByUserIdCommand _getImagesWithSecretTextByUserIdCommand;


    public ImageService(IAddImageAsyn addImageAsyn, IGetImgeByIdCommand getImgeByIdCommand,
        IGetListOfImageByUserIdCommend getListOfImageByUserIdCommend,IUpdateImageAsync updateImageAsync, 
        IDeleteImageById deleteImageById,ImageStore imageStore, IGetImagesWithSecretTextByUserIdCommand getImagesWithSecretTextByUserIdCommand )
    {
        _getImagesWithSecretTextByUserIdCommand=getImagesWithSecretTextByUserIdCommand;
        _updateImageAsync = updateImageAsync;
        ImageStore = imageStore;
        _deleteImageById = deleteImageById;
        _addImageAsyn = addImageAsyn;
        _getImgeByIdCommand = getImgeByIdCommand;
        _getListOfImageByUserIdCommend = getListOfImageByUserIdCommend;
    }


    public async Task<ImageDto?> AddImageAsync(ImageDto imageDto)
    {
        return await _addImageAsyn.AddImage(imageDto);
    }

    public async Task<ImageDto?> GetImageAsync(int id)
    {
        return await _getImgeByIdCommand.GetImgeById(id);

    }

    public async Task<List<ImageDto>> GetListOfImageByUserId(int id)
    {
        return await _getListOfImageByUserIdCommend.ExecuteAsync(id);

    }

    public async Task DeleteImageById(int id)
    {
        await _deleteImageById.DeleteImageByIdAsync(id);
    }

    public Task<ImageDto?> UpdateImageAsync(ImageDto imageDto)
    {
        return  _updateImageAsync.Execute(imageDto);
    }

    public void SetImageInStore(ImageDto imageDto)
    {
        ImageStore.SetImage(imageDto);
    }
    public ImageDto? GetImageFromStore()
    {
        return ImageStore.CurrentImage;
    }

    public Task<List<ImageDto>> GetListOfImageWithSecretText(int id)
    {
        return _getImagesWithSecretTextByUserIdCommand.ExecuteAsync(id);
    }
}