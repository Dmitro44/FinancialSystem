using FinancialSystem.Domain.Enums;
namespace FinancialSystem.Domain.Entities;

public abstract class Account
{
    public int Id { get; private set; }

    public decimal Balance { get; private set; }
    public Bank Bank { get; private set; }
    public int BankId { get; private set; }
    public AccountDiscriminator Discriminator { get; protected set; }
    public bool IsActive { get; private set; }

    public Account() {}
    public Account(decimal balance, Bank bank)
    {
        Balance = balance;
        Bank = bank;
        BankId = bank.Id;
        IsActive = true;
    }
    
    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void UpdateDetails(decimal balance)
    {
        Balance = balance;
    }

    public void Withdraw(decimal amount)
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("Account is not active");
        }
        
        if (amount <= 0)
            throw new ArgumentException("Amount cannot be negative");
        
        Balance -= amount;
    }

    public void Deposit(decimal amount)
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("Account is not active");
        }
        
        if (amount <= 0)
            throw new ArgumentException("Amount cannot be negative");
        
        Balance += amount;
    }
}