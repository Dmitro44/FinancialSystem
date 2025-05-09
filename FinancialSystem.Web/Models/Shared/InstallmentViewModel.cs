using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Web.Models.Shared;

public class InstallmentViewModel
{
    public int BankId { get; set; }
    public decimal Amount { get; set; }
    public int TermInMonths { get; set; }
    public decimal MonthlyPayment { get; set; }
    
    public int DestinationAccountId { get; set; }
    public List<UserAccount> UserAccounts { get; set; }
}