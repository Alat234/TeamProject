using Microsoft.EntityFrameworkCore;
using TeamProject.DbContexts;
using TeamProject.Models;
using System.Data.Common;
namespace TeamProject.Repositorie;
 
public interface IUserRepository
{
    Task <UserEntity?> GetUserById(int id);
    Task <UserEntity?> GetUserByEmail(string email);
    Task  Update(UserEntity user);
    Task<UserEntity?> AddUser(UserEntity user);
    
    
    

}
public class UserRepository : IUserRepository
{
    private readonly BmpEditorDbContext _context;
    public UserRepository( DbContextFactory dbContextFactory)
    {
        _context = dbContextFactory.CreateContext();
        
    }
   
    public  async Task <UserEntity?> GetUserById(int id)
    {
       
        return await _context.Users
            .Include(u => u.Images) 
            .FirstOrDefaultAsync(u => u.Id == id);

    }

    public async Task<UserEntity?> GetUserByEmail(string email)
    {
        UserEntity? user = new UserEntity();
        user=await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
        return user;
    }

    public async Task Update(UserEntity user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public  async Task<UserEntity?> AddUser(UserEntity user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return await  GetUserById(user.Id);
        
    }
}