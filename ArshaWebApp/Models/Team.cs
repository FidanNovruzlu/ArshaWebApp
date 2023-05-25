using System.ComponentModel.DataAnnotations;

namespace ArshaWebApp.Models;
public class Team
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    [MaxLength(200)]
    public string Bio { get; set; } = null!;
    public string ProfileImageName { get; set; } = null!;
    public int JobId { get; set; }
    public Job Job { get; set; } = null!;
}