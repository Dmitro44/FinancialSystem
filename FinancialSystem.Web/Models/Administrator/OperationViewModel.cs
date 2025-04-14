using FinancialSystem.Domain.Operations;

namespace FinancialSystem.Web.Models.Administrator;

public class OperationViewModel
{
    public int BankId { get; set; }
    public int LogId { get; set; }
    public string? Comment { get; set; }
    
    public List<OperationLog> Operations { get; set; }
}