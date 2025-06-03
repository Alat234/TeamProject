using TeamProject.Commends.UserCommends;
using TeamProject.DTOs;
using TeamProject.Models;

namespace TeamProject.Services;
public interface IUserService
{
    Task<UserDto> RegisterUserAsync(UserDto userDto);
    Task<UserDto?> LoginUserAsync(UserDto userDto);
    Task<UserDto?> GetUserById(int  userId);
    
    
    

}
public class UserService : IUserService{
    
   private readonly IRegistrationCommend _addUserCommand;
   private readonly ILoginCommend _loginCommend;
   private readonly IGetUserByIdCommand _getUserByIdCommand;

   public UserService(IRegistrationCommend addUserCommand, ILoginCommend loginCommend,IGetUserByIdCommand getUserByIdCommand)
    {
        _addUserCommand = addUserCommand;
        _loginCommend = loginCommend;
        _getUserByIdCommand = getUserByIdCommand;
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

    public async Task<UserDto?> GetUserById(int userId)
    {
       return await _getUserByIdCommand.ExecuteAsync(userId);
    }
}