using System.ComponentModel.DataAnnotations;

namespace FuneralHome.DTOs;

public class LoginModel
{
    public string Mail { get; set; } = string.Empty;
    [Required(ErrorMessage = "Password can't be null")]
    [StringLength(50, ErrorMessage = "Password can't be longer than 50 characters")]
    public string Lozinka { get; set; } = string.Empty;
}

