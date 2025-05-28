using AutoMapper;
using TeamProject.DTOs;
using TeamProject.Models;
using TeamProject.Repositorie;

namespace TeamProject.Commends.ImageDbCommand;

public interface IAddImageAsyn
{
    Task<ImageDto?> AddImage(ImageDto image);
}
public class AddImageAsyn: IAddImageAsyn
{
    private readonly  IImageRepository _imageRepository;
    private readonly IMapper _mapper;

    public AddImageAsyn(IImageRepository imageRepository,IMapper mapper)
    {
        _imageRepository=imageRepository;
        _mapper=mapper;
        
    }

    public async Task<ImageDto?> AddImage(ImageDto image)
    {
        var imageEntity = _mapper.Map<ImageEntity>(image);
        ImageEntity ? imageEntity1 = await _imageRepository.GetImageById(imageEntity.Id);
        if( imageEntity1 != null)
        {
            return null;
        }
        return  _mapper.Map<ImageDto>(await _imageRepository.AddImage(imageEntity));
        
       
    }
}
