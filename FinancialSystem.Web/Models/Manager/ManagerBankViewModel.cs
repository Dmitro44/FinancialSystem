using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Web.Models.Manager;

public class ManagerBankViewModel
{
    public int BankId { get; set; }
    public string BankName { get; set; }

    public List<Loan> LoanRequests = new();
    public List<Installment> InstallmentRequests = new();
}