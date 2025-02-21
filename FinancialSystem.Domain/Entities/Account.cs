namespace FinancialSystem.Domain.Entities;

public class Account
{
    public int Id { get; private set; }
    public User Owner { get; private set; }
    public int OwnerId { get; private set; }
    public decimal Balance { get; private set; }
    public Bank Bank { get; private set; }
    public int BankId { get; private set; }

    public Account() {}
    public Account(User owner, decimal balance, Bank bank)
    {
        Owner = owner;
        OwnerId = owner.Id;
        Balance = balance;
        Bank = bank;
        BankId = bank.Id;
    }
}