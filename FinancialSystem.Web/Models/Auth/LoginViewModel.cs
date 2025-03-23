using System.ComponentModel.DataAnnotations;

namespace FinancialSystem.Web.Models;

public class LoginViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; }
    
    [Required, DataType(DataType.Password)]
    public string Password { get; set; }
}