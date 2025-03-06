using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Web.Models;

public class BanksViewModel
{
    public List<UserBankRole> RegisteredBanks { get; set; }
    public List<Bank> OtherBanks { get; set; }
    public List<string> AvailableRoles = Enum.GetNames(typeof(Role)).ToList();
}