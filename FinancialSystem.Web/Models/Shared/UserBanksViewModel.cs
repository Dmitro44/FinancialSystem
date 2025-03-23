using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;

namespace FinancialSystem.Web.Models.Shared;

public class UserBanksViewModel
{
    public List<UserBankRole> RegisteredBanks { get; set; }
    public List<Bank> OtherBanks { get; set; }
    public List<string> AvailableRoles = Enum.GetNames(typeof(Role)).ToList();
}