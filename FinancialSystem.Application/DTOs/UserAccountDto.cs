namespace FinancialSystem.Application.DTOs;

public class UserAccountDto
{
    public int OwnerId { get; set; }
    public int BankId { get; set; }
    public decimal Balance { get; set; }
}