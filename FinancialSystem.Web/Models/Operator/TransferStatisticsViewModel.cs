using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Web.Models.Operator;

public class TransferStatisticsViewModel
{
    public List<Transfer> TransfersFromBank { get; set; }
    
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }
}