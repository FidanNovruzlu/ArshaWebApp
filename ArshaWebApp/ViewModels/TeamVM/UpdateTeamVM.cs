using ArshaWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ArshaWebApp.ViewModels.TeamVM;
public class UpdateTeamVM
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    [MaxLength(200)]
    public string? Bio { get; set; }
    public int JobId { get; set; }
    public List<Job>? Jobs { get; set; }
    public IFormFile? ProfileImage { get; set; }
    public string? ProfileImageName { get; set; } 
}