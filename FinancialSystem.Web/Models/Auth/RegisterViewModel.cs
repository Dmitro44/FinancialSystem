using System.ComponentModel.DataAnnotations;

namespace FinancialSystem.Web.Models;

public class RegisterViewModel
{
    [Required]
    [RegularExpression(@"^[A-ZА-ЯЁ][a-zа-яё]+$", ErrorMessage = "The first letter must be uppercase.")]
    public string FirstName { get; set; }

    [Required]
    [RegularExpression(@"^[A-ZА-ЯЁ][a-zа-яё]+$", ErrorMessage = "The first letter must be uppercase.")]
    public string LastName { get; set; }

    [Required]
    [RegularExpression(@"^[A-ZА-ЯЁ][a-zа-яё]+$", ErrorMessage = "The first letter must be uppercase.")]
    public string Patronymic { get; set; }

    [Required]
    [RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "Must be exactly 2 uppercase letters.")]
    public string PassportSeries { get; set; }

    [Required]
    [RegularExpression(@"^\d{7}$", ErrorMessage = "Must be exactly 7 digits.")]
    public string PassportNumber { get; set; }

    [Required]
    [RegularExpression(@"^[0-9A-Z]{14}$", ErrorMessage = "Identification number must contain exactly 14 characters (digits and capital letters).")]
    public string IdentificationNumber { get; set; }
    
    [Required, Phone]
    [RegularExpression(@"^\+[0-9]+$", ErrorMessage = "Phone number must start with + followed by digits.")]
    public string PhoneNumber { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }
    
    [Required, DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required, DataType(DataType.Password), Compare("Password")]
    public string ConfirmPassword { get; set; }
}