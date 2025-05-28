using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TeamProject.DbContexts;
using TeamProject.DTOs;
using TeamProject.Models;
using TeamProject.Repositorie;

namespace TeamProject.Commends.UserCommends;
public interface IRegistrationCommend
{
    Task  <UserDto>RegistrationUser(UserDto userDto);
}

public class RegistrationCommend:IRegistrationCommend
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;


    public RegistrationCommend(IUserRepository userRepository, IMapper mapper )
    {
        _userRepository=userRepository;
        _mapper=mapper;        
    }
    public async Task <UserDto> RegistrationUser(UserDto userDto)
    {
        ArgumentNullException.ThrowIfNull(userDto);
        var existingUser = await _userRepository.GetUserByEmail(userDto.Email);
        if (existingUser != null)
            throw new ArgumentException($"User with email {userDto.Email} already exists.");
        var userEntity = _mapper.Map<UserEntity>(userDto);
        await _userRepository.AddUser(userEntity);
        var resultDto = _mapper.Map<UserDto>(userEntity);
        return resultDto;
    }
    
}