namespace FinancialSystem.Application.DTOs;

public class SalaryProjectDto
{
    public int EnterpriseId { get; set; }
    public int EnterpriseAccountId { get; set; }
    public decimal Salary { get; set; }
    public int BankId { get; set; }
}