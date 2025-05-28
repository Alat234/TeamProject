using AutoMapper;
using TeamProject.DTOs;
using TeamProject.Models;
using TeamProject.Repositorie;

namespace TeamProject.Commends.ImageDbCommand;

public interface  IUpdateImageAsync
{
    Task <ImageDto?> Execute(ImageDto  imageDto);
}


public class UpdateImageAsync: IUpdateImageAsync
{
    private readonly IImageRepository _imageRepository;
    private readonly IMapper _maper;


    public UpdateImageAsync(IImageRepository? imageRepository,IMapper mapper)
    {
        _imageRepository = imageRepository;
        _maper= mapper;
    }
    
    public async Task<ImageDto?> Execute(ImageDto  imageDto)
    {
        ImageEntity image = _maper.Map<ImageEntity>(imageDto);
         var result = await _imageRepository.UpdateImage(image);
        if (result == null)
        {
            return null;
        }
        return _maper.Map<ImageDto>(result);
        
    }
}