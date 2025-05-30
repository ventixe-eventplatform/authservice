using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

public class EmailRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
}
