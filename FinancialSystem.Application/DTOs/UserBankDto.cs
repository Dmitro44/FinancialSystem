using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;

namespace FinancialSystem.Application.DTOs;

public class UserBankDto
{
    public int UserId { get; set; }
    public int BankId { get; set; }
    public Role Role { get; set; }
}