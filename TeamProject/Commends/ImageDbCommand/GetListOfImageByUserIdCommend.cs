using AutoMapper;
using TeamProject.DTOs;
using TeamProject.Repositorie;

namespace TeamProject.Commends.ImageDbCommand;

public interface IGetListOfImageByUserIdCommend
{
    Task<List<ImageDto>> ExecuteAsync(int userId);
}

public class GetListOfImageByUserIdCommend: IGetListOfImageByUserIdCommend
{
    private readonly IImageRepository _imageRepository;
    private readonly IMapper _mapper;


    public GetListOfImageByUserIdCommend(IImageRepository  imageRepository , IMapper mapper)
    {
        _imageRepository = imageRepository;
        _mapper = mapper;
       
    }

    public async Task<List<ImageDto>> ExecuteAsync(int userId)
    {
        var imageEntities = await _imageRepository.GetImagesByUserId(userId);
        return _mapper.Map<List<ImageDto>>(imageEntities);
       
    }
}
