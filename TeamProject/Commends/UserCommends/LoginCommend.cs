using AutoMapper;
using TeamProject.DTOs;
using TeamProject.Models;
using TeamProject.Repositorie;

namespace TeamProject.Commends.UserCommends;

public interface ILoginCommend
{
    Task<UserDto?> LoginUser(UserDto userDto);

}
public class LoginCommend : ILoginCommend
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public LoginCommend(IUserRepository repository,  IMapper mapper)
    {
        _userRepository = repository;
        _mapper = mapper;
    }
    public async Task<UserDto?> LoginUser(UserDto userDto)
    {
        ArgumentNullException.ThrowIfNull(userDto);
        var existingUser = await _userRepository.GetUserByEmail(userDto.Email);
        if (existingUser == null)
        {
            return null;
        }
        var resultDto = _mapper.Map<UserDto>(existingUser);
        return resultDto;

    }
}
