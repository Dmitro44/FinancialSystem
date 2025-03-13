namespace FinancialSystem.Domain.Entities;

public class Bank : BaseEnterprise
{
    public string Bic { get; private set; }
    public List<UserBankRole> UserBankRoles { get; private set; } = new();
    
    public List<Enterprise> Enterprises { get; private set; } = new();
    public List<Account> Accounts { get; private set; } = new();
    public List<Loan> Loans { get; private set; } = new();
    public List<Installment> Installments { get; private set; } = new();
    public List<Transfer> Transfers { get; private set; } = new();
    
    public Bank() : base() {}
    public Bank(string name, string bic, string address)
        : base(name, address)
    {
        Bic = bic;
    }

    public void UpdateDetails(string name, string bic, string address)
    {
        Bic = bic;
        base.UpdateDetails(name, address);
    }
}