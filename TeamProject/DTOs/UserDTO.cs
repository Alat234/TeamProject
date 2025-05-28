using System.ComponentModel.DataAnnotations;
using TeamProject.Models;

namespace TeamProject.DTOs;

public class UserDto
{
    [Key]
    public int Id{get;set;}
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public List<ImageDto> Images { get; set; } = [];

}