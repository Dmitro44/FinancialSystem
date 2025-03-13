using Microsoft.AspNetCore.Identity;

namespace FinancialSystem.Domain.Entities;

public class User
{
    public int Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Patronymic { get; private set; }
    public string PassportNumber { get; private set; }
    public string PassportSeries { get; private set; }
    public string IdentificationNumber { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    
    // Add smth for foreign clients
    
    public List<UserBankRole> UserBankRoles { get; private set; } = new();
    public User() {}

    public User(string firstName, string lastName, string patronymic,
        string passportNumber, string passportSeries, string identificationNumber,
        string phoneNumber, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Patronymic = patronymic;
        PassportNumber = passportNumber;
        PassportSeries = passportSeries;
        IdentificationNumber = identificationNumber;
        PhoneNumber = phoneNumber;
        Email = email;
    }

    public void UpdateDetails(string firstName, string lastName, string patronymic,
        string passportNumber, string passportSeries, string identificationNumber,
        string phoneNumber, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Patronymic = patronymic;
        PassportNumber = passportNumber;
        PassportSeries = passportSeries;
        IdentificationNumber = identificationNumber;
        PhoneNumber = phoneNumber;
        Email = email;
    }

    public void SetPassword(string userPassword)
    {
        PasswordHash = new PasswordHasher<User>().HashPassword(this, userPassword);
    }

    public bool VerifyPassword(string userPassword)
    {
        var result = new PasswordHasher<User>().VerifyHashedPassword(this, PasswordHash, userPassword);

        if (result == PasswordVerificationResult.Success)
        {
            return true;
        }

        return false;
    }
}