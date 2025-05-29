using AutoMapper;
using TeamProject.DTOs;
using TeamProject.Repositorie;

namespace TeamProject.Commends.ImageDbCommand;
public interface IGetImagesWithSecretTextByUserIdCommand
{
    public Task<List<ImageDto>> ExecuteAsync(int userId);
    
    
}


public class GetImagesWithSecretTextByUserIdCommand:IGetImagesWithSecretTextByUserIdCommand
{
    private readonly IImageRepository _imageRepository;
    private readonly IMapper _mapper;

    public GetImagesWithSecretTextByUserIdCommand(IImageRepository imageRepository, IMapper mapper)
    {
        _imageRepository=imageRepository;
        _mapper=mapper;
        
    }


    public async Task<List<ImageDto>> ExecuteAsync(int userId)
    {
        var imageEntities = await _imageRepository.GetImagesWithSecretTextByUserId(userId);
        return _mapper.Map<List<ImageDto>>(imageEntities);
        
        
    }
}