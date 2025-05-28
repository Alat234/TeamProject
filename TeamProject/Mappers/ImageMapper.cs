using System.IO;
using System.Windows.Media.Imaging;
using AutoMapper;
using TeamProject.DTOs;
using TeamProject.Models;

namespace TeamProject.Mappers;

public class ImageMapper : Profile
{
    public ImageMapper()
    {
        //  ImageEntity → ImageDTO
        CreateMap<ImageEntity, ImageDto>()
            .ForMember(dest => dest.ImageSource, 
                opt => opt.MapFrom(src => ConvertByteArrayToBitmapSource(src.ImageData)));

        // М ImageDTO → ImageEntity
        CreateMap<ImageDto, ImageEntity>()
            .ForMember(dest => dest.ImageData, 
                opt => opt.MapFrom(src => ConvertBitmapSourceToByteArray(src.ImageSource)));
    }

    private BitmapSource ConvertByteArrayToBitmapSource(byte[] byteArray)
    {
        if (byteArray == null || byteArray.Length == 0) return null;

        using (MemoryStream stream = new MemoryStream(byteArray))
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();
            bitmapImage.Freeze();
            return bitmapImage;
        }
    }

    private byte[] ConvertBitmapSourceToByteArray(BitmapSource bitmapSource)
    {
        if (bitmapSource == null) return null;

        BmpBitmapEncoder encoder = new BmpBitmapEncoder(); // Можна змінити на PngBitmapEncoder
        encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

        using (MemoryStream stream = new MemoryStream())
        {
            encoder.Save(stream);
            return stream.ToArray();
        }
    }
    
}