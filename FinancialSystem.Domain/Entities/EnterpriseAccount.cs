using FinancialSystem.Domain.Enums;

namespace FinancialSystem.Domain.Entities;

public class EnterpriseAccount : Account
{
    public int EnterpriseId { get; private set; }
    public Enterprise Enterprise { get; private set; }

    public EnterpriseAccount()
    {
        Discriminator = AccountDiscriminator.Enterprise;
    }

    public EnterpriseAccount(Enterprise enterprise, decimal balance, Bank bank)
        :base(balance, bank)
    {
        Enterprise = enterprise;
        EnterpriseId = enterprise.Id;
        Discriminator = AccountDiscriminator.Enterprise;
    }

    public new void UpdateDetails(decimal balance)
    {
        base.UpdateDetails(balance);
    }
}