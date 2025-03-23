using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Web.Models.Client;

public class TransferViewModel
{
    public int BankId { get; set; }

    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public decimal Amount { get; set; }
    
    public List<Account> FromAccounts { get; set; }

    public string? ErrorMessage { get; set; }
}