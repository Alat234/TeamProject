using AutoMapper;
using TeamProject.DTOs;
using TeamProject.Repositorie;

namespace TeamProject.Commends.UserCommends;

public interface IGetUserByIdCommand
{
    Task<UserDto> ExecuteAsync(int userId);
}

public class GetUserByIdCommand:IGetUserByIdCommand
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public GetUserByIdCommand(IMapper mapper,IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        
    }
    public async Task<UserDto> ExecuteAsync(int userId)
    {
        var userEntity= await _userRepository.GetUserById(userId);
        return _mapper.Map<UserDto>(userEntity);
    }
}