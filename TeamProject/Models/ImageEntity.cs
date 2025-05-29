using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace TeamProject.Models;

public class ImageEntity 
{
    [Key]
    public int Id { get; set; }
    
    public byte[]? ImageData { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool HasSecretText { get; set; }
    

  
  
    
}