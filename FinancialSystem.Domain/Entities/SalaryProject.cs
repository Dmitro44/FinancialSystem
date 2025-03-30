using FinancialSystem.Domain.Enums;

namespace FinancialSystem.Domain.Entities;

public class SalaryProject
{
    public int Id { get; private set; }
    public int EnterpriseId { get; private set; }
    public int EnterpriseAccountId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public SalaryProjectStatus Status { get; private set; }
    public Enterprise Enterprise { get; private set; }
    public EnterpriseAccount EnterpriseAccount { get; private set; }
    public decimal Salary { get; private set; }
    public int BankId { get; private set; }
    public Bank Bank { get; private set; }
    
    public SalaryProject() {}

    public SalaryProject(Enterprise enterprise, EnterpriseAccount enterpriseAccount, 
        decimal salary, Bank bank)
    {
        Enterprise = enterprise;
        EnterpriseId = enterprise.Id;
        EnterpriseAccount = enterpriseAccount;
        EnterpriseAccountId = enterpriseAccount.Id;
        CreatedAt = DateTime.UtcNow.AddHours(3);
        Status = SalaryProjectStatus.Pending;
        Salary = salary;
        Bank = bank;
        BankId = bank.Id;
    }

    public void SetStatus(SalaryProjectStatus status)
    {
        if (status == SalaryProjectStatus.Pending) return;
        
        Status = status;
    }
}