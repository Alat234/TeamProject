using System.ComponentModel.DataAnnotations;
using System.Windows.Media.Imaging;

namespace TeamProject.DTOs;

public class ImageDto
{
    [Key]
    public int Id{get;set;}
    public BitmapSource ImageSource { get; set; }
    public DateTime? CreatedAt { get; set; }
    public bool HasSecretText { get; set; }
    public int UserId { get; set; }
   

}