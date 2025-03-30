using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Web.Models.Client;

public class ClientFinancesBankViewModel
{
    public int BankId { get; set; }
    public string BankName { get; set; }
    public string Bic { get; set; }         // not used
    public string Address { get; set; }

    public List<UserAccount> Accounts = new();
    public List<Loan> Loans = new();
    public List<Installment> Installments = new();
}