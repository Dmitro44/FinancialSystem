namespace FinancialSystem.Domain.Entities;

public class SalaryProject
{
    public int Id { get; private set; }
    public int EnterpriseId { get; private set; }
    public int EnterpriseAccountId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }
    public Enterprise Enterprise { get; private set; }
    public Account EnterpriseAccount { get; private set; }
    
    public List<Account> EmployeeAccounts { get; private set; } = new();
    
    public SalaryProject() {}

    public SalaryProject(DateTime createdAt, bool isActive, Enterprise enterprise, Account enterpriseAccount)
    {
        Enterprise = enterprise;
        EnterpriseId = enterprise.Id;
        EnterpriseAccount = enterpriseAccount;
        EnterpriseAccountId = enterpriseAccount.Id;
        CreatedAt = createdAt;
        IsActive = isActive;
    }
}