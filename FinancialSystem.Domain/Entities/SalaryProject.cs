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
    public Account EnterpriseAccount { get; private set; }
    
    public List<Account> EmployeeAccounts { get; private set; } = new();
    
    public SalaryProject() {}

    public SalaryProject(DateTime createdAt, Enterprise enterprise, Account enterpriseAccount)
    {
        Enterprise = enterprise;
        EnterpriseId = enterprise.Id;
        EnterpriseAccount = enterpriseAccount;
        EnterpriseAccountId = enterpriseAccount.Id;
        CreatedAt = createdAt;
        Status = SalaryProjectStatus.Pending;
    }

    public void SetStatus(SalaryProjectStatus status)
    {
        if (status == SalaryProjectStatus.Pending) return;
        
        Status = status;
    }
}