using FinancialSystem.Domain.Enums;

namespace FinancialSystem.Domain.Entities;

public class UserBankRole
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public User User { get; private set; }
    public int BankId { get; private set; }
    public Bank Bank { get; private set; }
    public Role Role { get; private set; }
    
    public UserBankRole() { }

    public UserBankRole(User user, Bank bank, Role role)
    {
        User = user;
        Bank = bank;
        UserId = user.Id;
        BankId = bank.Id;
        Role = role;
    }
}