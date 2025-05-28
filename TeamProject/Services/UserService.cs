using TeamProject.Commends.UserCommends;
using TeamProject.DTOs;
using TeamProject.Models;

namespace TeamProject.Services;
public interface IUserService
{
    Task<UserDto> RegisterUserAsync(UserDto userDto);
    Task<UserDto?> LoginUserAsync(UserDto userDto);
    
    
    

}
public class UserService : IUserService{
    
   private readonly IRegistrationCommend _addUserCommand;
   private readonly ILoginCommend _loginCommend;

   public UserService(IRegistrationCommend addUserCommand, ILoginCommend loginCommend)
    {
        _addUserCommand = addUserCommand;
        _loginCommend = loginCommend;
    }
    
    public async Task<UserDto> RegisterUserAsync(UserDto userDto)
    {
        return await _addUserCommand.RegistrationUser(userDto);
    }

    public async Task<UserDto?> LoginUserAsync(UserDto userDto)
    {
        var tempUserDto=await _loginCommend.LoginUser(userDto);
        return tempUserDto;
    }
}