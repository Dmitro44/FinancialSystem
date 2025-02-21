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
    public string? PhoneNumber { get; private set; }
    public string? Email { get; private set; }
    public Role Role { get; private set; }
    
    // Add smth for foreign clients

    public List<Bank> Banks { get; private set; } = new();
    
    public User() {}

    public User(string firstName, string lastName, string patronymic,
        string passportNumber, string passportSeries, string identificationNumber,
        string phoneNumber, string email, Role role)
    {
        FirstName = firstName;
        LastName = lastName;
        Patronymic = patronymic;
        PassportNumber = passportNumber;
        PassportSeries = passportSeries;
        IdentificationNumber = identificationNumber;
        PhoneNumber = phoneNumber;
        Email = email;
        Role = role;
    }
}

public enum Role
{
    Client,
    Operator,
    Manager,
    EnterpriseSpecialist,
    Administrator
}