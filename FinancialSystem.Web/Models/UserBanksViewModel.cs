using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Web.Models;

public class UserBanksViewModel
{
    public List<UserBankRole> RegisteredBanks { get; set; }
    public List<Bank> OtherBanks { get; set; }
    public List<string> AvailableRoles = Enum.GetNames(typeof(Role)).ToList();
}