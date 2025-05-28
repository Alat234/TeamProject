using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace TeamProject.Models;

public class UserEntity
{
    
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public List<ImageEntity> Images { get; set; } = new List<ImageEntity>();
}

    
  


    
