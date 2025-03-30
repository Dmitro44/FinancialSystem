namespace FinancialSystem.Domain.Entities;

public class Account
{
    public int Id { get; private set; }
    public decimal Balance { get; private set; }
    public Bank Bank { get; private set; }
    public int BankId { get; private set; }

    public Account() {}
    public Account(decimal balance, Bank bank)
    {
        Balance = balance;
        Bank = bank;
        BankId = bank.Id;
    }

    public void UpdateDetails(decimal balance)
    {
        Balance = balance;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount cannot be negative");
        
        Balance -= amount;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount cannot be negative");
        
        Balance += amount;
    }
}