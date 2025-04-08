using FinancialSystem.Domain.Enums;

namespace FinancialSystem.Domain.Entities;

public class UserAccount : Account
{
    public User Owner { get; private set; }
    public int OwnerId { get; private set; }
    public AccountType AccountType { get; private set; }
    
    public int? EmployerEnterpriseId { get; private set; }
    public Enterprise EmployerEnterprise { get; private set; }

    public UserAccount()
    {
        Discriminator = AccountDiscriminator.User;
    }

    public UserAccount(User owner, decimal balance, Bank bank, AccountType type)
        :base(balance, bank)
    {
        Owner = owner;
        OwnerId = owner.Id;
        AccountType = type;
        Discriminator = AccountDiscriminator.User;
    }

    public void SetEmployer(Enterprise employer)
    {
        EmployerEnterprise = employer;
        EmployerEnterpriseId = employer.Id;
    }
}