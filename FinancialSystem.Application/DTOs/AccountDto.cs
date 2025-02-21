namespace FinancialSystem.Application.DTOs;

public class AccountDto
{
    public int UserId { get; set; }
    public int BankId { get; set; }
    public decimal Balance { get; set; }
}