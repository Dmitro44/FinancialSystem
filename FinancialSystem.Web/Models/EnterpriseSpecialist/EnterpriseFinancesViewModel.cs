using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Web.Models.EnterpriseSpecialist;

public class EnterpriseFinancesViewModel
{
    public int BankId { get; set; }
    
    public string EnterpriseName { get; set; }
    public string EnterpriseUnp { get; set; }
    public string EnterpriseType { get; set; }
    public string EnterpriseAddress { get; set; }

    public List<EnterpriseAccount> EnterpriseAccounts { get; set; }
    public List<SalaryProject> SalaryProjects { get; set; }
}