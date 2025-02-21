namespace FinancialSystem.Domain.Entities;

public class Enterprise : BaseEnterprise
{
    public string Type { get; private set; }
    public string Unp { get; private set; }
    public Bank Bank { get; private set; }
    public int BankId { get; private set; }
    
    public List<Account> Accounts { get; private set; } = new();
    
    public Enterprise() : base() {}

    public Enterprise(string type, string name, string unp, string address, Bank bank)
        : base(name, address)
    {
        Type = type;
        Unp = unp;
        Bank = bank;
        BankId = bank.Id;
    }
}