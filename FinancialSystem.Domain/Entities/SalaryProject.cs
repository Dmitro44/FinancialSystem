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
    public bool IsActive { get; private set; }

    public List<SalaryProjectEmployee> Employees { get; private set; } = new();
    
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
        IsActive = true;
    }

    public void UpdateDetails(decimal salary)
    {
        Salary = salary;
    }

    public void SetStatus(SalaryProjectStatus status)
    {
        if (status == SalaryProjectStatus.Pending) return;
        
        Status = status;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}