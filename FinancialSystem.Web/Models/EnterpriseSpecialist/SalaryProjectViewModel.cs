using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Web.Models.EnterpriseSpecialist;

public class SalaryProjectViewModel
{
    public int BankId { get; set; }
    public int EnterpriseAccountId { get; set; }
    public decimal Salary { get; set; }

    public List<EnterpriseAccount> EnterpriseAccounts { get; set; }
}