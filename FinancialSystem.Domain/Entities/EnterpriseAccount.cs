namespace FinancialSystem.Domain.Entities;

public class EnterpriseAccount : Account
{
    public int EnterpriseId { get; private set; }
    public Enterprise Enterprise { get; private set; }
    
    public EnterpriseAccount() {}

    public EnterpriseAccount(Enterprise enterprise, decimal balance, Bank bank)
        :base(balance, bank)
    {
        Enterprise = enterprise;
        EnterpriseId = enterprise.Id;
    }

    public void UpdateDetails(decimal balance)
    {
        UpdateDetails(balance);
    }
}