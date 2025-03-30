namespace FinancialSystem.Domain.Entities;

public class UserAccount : Account
{
    public User Owner { get; set; }
    public int OwnerId { get; set; }

    public UserAccount() {}

    public UserAccount(User owner, decimal balance, Bank bank)
        :base(balance, bank)
    {
        Owner = owner;
    }
}