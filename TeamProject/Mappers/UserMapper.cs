using AutoMapper;
using TeamProject.DTOs;
using TeamProject.Models;

namespace TeamProject.Mappers;

public class UserMapper: Profile
{
    public UserMapper()
    {
        CreateMap<UserEntity, UserDto>();
        CreateMap<UserDto, UserEntity>();
        //  UserDTOForSerialization → UserDTO
        CreateMap<UserDtoForSerialization, UserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore()); 

        //  UserDTO → UserDTOForSerialization 
        CreateMap<UserDto, UserDtoForSerialization>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

    }
    
}