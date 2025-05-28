using AutoMapper;
using TeamProject.DTOs;
using TeamProject.Models;
using TeamProject.Repositorie;

namespace TeamProject.Commends.ImageDbCommand;
public interface  IGetImgeByIdCommand
{
    Task<ImageDto?>  GetImgeById (int id);

}

public class GetImageByIdCommand:IGetImgeByIdCommand
{
    private readonly IImageRepository _imageRepository;
    private readonly IMapper _mapper;

    public GetImageByIdCommand(IImageRepository imageRepository,IMapper mapper )
    {
        _imageRepository=imageRepository;
        _mapper=mapper;
        
    }

    public async Task<ImageDto?> GetImgeById(int id)
    {
        var imageEntity= await _imageRepository.GetImageById( id);
        if (imageEntity == null)
        {
            return null;
        }
        return  _mapper.Map<ImageDto>(imageEntity);
        
        
    }
}