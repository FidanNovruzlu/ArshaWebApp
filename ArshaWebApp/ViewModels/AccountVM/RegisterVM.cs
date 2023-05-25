using System.ComponentModel.DataAnnotations;

namespace ArshaWebApp.ViewModels.AccountVM;
public class RegisterVM
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    [Required,MinLength(8),MaxLength(20)]
    public string Username { get; set; } = null!;
    [Required,MinLength(8),DataType(DataType.Password)]
    public string Password { get; set; }= null!;
    [Required,MinLength(8),DataType(DataType.Password),Compare(nameof(Password))]
    public string ConfrimPassword { get; set; } = null!;
    [EmailAddress,Required]
    public string Email { get; set; }=null!;
}
