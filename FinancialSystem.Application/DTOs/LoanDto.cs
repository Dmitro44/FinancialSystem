namespace FinancialSystem.Application.DTOs;

public class LoanDto
{
    public int UserId { get; set; }
    public int BankId { get; set; }
    public decimal Amount { get; set; }
    public int TermInMonths { get; set; }
    public decimal InterestRate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal MonthlyPayment { get; set; }

    public int DestinationAccountId { get; set; }
}