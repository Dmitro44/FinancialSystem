namespace FinancialSystem.Application.DTOs;

public class BaseCalculationRequest
{
    public decimal Amount { get; set; }
    public int TermInMonths { get; set; }
}