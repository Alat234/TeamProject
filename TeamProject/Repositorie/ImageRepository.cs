using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using TeamProject.DbContexts;
using TeamProject.Models;

namespace TeamProject.Repositorie;

public interface IImageRepository
{
    Task<ImageEntity?> GetImageById(int id);
    Task<List<ImageEntity>> GetImagesByUserId(int userId);
    Task<ImageEntity?> AddImage(ImageEntity image);
    Task<ImageEntity?> UpdateImage(ImageEntity image);
    Task DeleteImage(int id);
    Task<List<ImageEntity>> GetImagesWithSecretTextByUserId(int userId);
}
public class ImageRepository: IImageRepository
{
    private readonly BmpEditorDbContext _context;

    public ImageRepository( DbContextFactory dbContextFactory)
    {
        _context=dbContextFactory.CreateContext();
        
    }
    public  async  Task  <ImageEntity?> GetImageById(int id)
    {
        return await _context.Images
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task <List<ImageEntity>> GetImagesByUserId(int userId)
    {
        return await _context.Images
            .Where(i => i.UserId == userId) 
            .ToListAsync();;
        
    }

    public async Task<ImageEntity?> AddImage(ImageEntity image)
    {
        image.CreatedAt = DateTime.UtcNow; 
        _context.Images.Add(image);
        await _context.SaveChangesAsync();
        return await GetImageById(image.Id);
    }

    public async Task<ImageEntity?> UpdateImage(ImageEntity image)
    {
        _context.Images.Update(image);
        await _context.SaveChangesAsync();
        return await GetImageById(image.Id);
    }

    public async Task DeleteImage(int id)
    {
        var image = await GetImageById(id);
        if (image != null)
        {
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<ImageEntity>> GetImagesWithSecretTextByUserId(int userId)
    {
        return await _context.Images
            .Where(i => i.UserId == userId && i.HasSecretText) 
            .ToListAsync();;
    }
}