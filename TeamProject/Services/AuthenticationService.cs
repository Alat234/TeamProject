using System.IO;
using System.Text.Json;
using AutoMapper;
using TeamProject.DTOs;

namespace TeamProject.Services;

public interface  IAuthenticationService
{
    UserDto CurrentUser { get; }
    void Login(UserDto user);
    void Logout();
    bool IsLoggedIn { get; }
    void SaveUser();
    void LoadUser();

}
public class AuthenticationService: IAuthenticationService
{
    private UserDto _currentUser;
    private readonly IMapper _mapper;
    private const string FilePath = "user.json";

    public AuthenticationService(IMapper mapper)
    {
        _mapper = mapper;
    }
    public UserDto CurrentUser
    {
        get => _currentUser;
        private set => _currentUser = value;
    }
    public void Login(UserDto user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        CurrentUser = user;
        SaveUser();
    }

    public void Logout()
    {
        CurrentUser = null;
        SaveUser();
    }

    public bool IsLoggedIn => CurrentUser != null;
    public void  SaveUser()
    {
        string jsonString = JsonSerializer.Serialize( _mapper.Map<UserDtoForSerialization>( CurrentUser));
        File.WriteAllText(FilePath, jsonString);
    }

    public void LoadUser()
    {
        if (File.Exists(FilePath))
        {
            string jsonString = File.ReadAllText(FilePath);
            CurrentUser = _mapper.Map<UserDto>( JsonSerializer.Deserialize<UserDtoForSerialization>(jsonString)) ;
        }
    }
}