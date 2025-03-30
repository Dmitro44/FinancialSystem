namespace FinancialSystem.Application.DTOs;

public class EnterpriseAccountDto
{
    public int EnterpriseId { get; set; }
    public int BankId { get; set; }
    public decimal Balance { get; set; }
}