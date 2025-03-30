using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Web.Models.Operator;

public class OperatorRequestsViewModel
{
    public int BankId { get; set; }
    public string BankName { get; set; }

    public List<SalaryProject> SalaryProjectRequests = new();
}