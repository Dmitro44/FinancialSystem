using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Web.Models;

public class BankViewModel
{
    public string BankName { get; set; }
    public string Bic { get; set; }
    public string Adress { get; set; }

    public List<Account> Accounts = new();
    public List<Loan> Loans = new();
    public List<Installment> Installments = new();
    
    public AccountViewModel NewAccount { get; set; }
}