using TeamProject.Repositorie;

namespace TeamProject.Commends.ImageDbCommand;
public interface  IDeleteImageById
{
    public Task DeleteImageByIdAsync(int id);
    

}
public class DeleteImageById: IDeleteImageById
{
    private readonly IImageRepository _imageRepository;

    public DeleteImageById(IImageRepository repository)
    {
        _imageRepository = repository;
       
    }
    public async Task  DeleteImageByIdAsync(int id)
    {
        await _imageRepository.DeleteImage(id);

    }
}
