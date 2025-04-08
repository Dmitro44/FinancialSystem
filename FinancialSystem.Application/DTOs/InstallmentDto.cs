namespace FinancialSystem.Application.DTOs;

public class InstallmentDto
{
    public int UserId { get; set; }
    public int BankId { get; set; }
    public decimal Amount { get; set; }
    public int TermInMonths { get; set; }
    public DateTime StartDate { get; set; }

    public int DestinationAccountId { get; set; }
}