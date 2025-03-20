namespace FinancialSystem.Web.Models;

public class InstallmentViewModel
{
    public int BankId { get; set; }
    public decimal Amount { get; set; }
    public int TermInMonths { get; set; }
    public decimal MonthlyPayment { get; set; }
}