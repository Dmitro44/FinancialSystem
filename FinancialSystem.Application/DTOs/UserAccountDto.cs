using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;

namespace FinancialSystem.Application.DTOs;

public class UserAccountDto
{
    public int OwnerId { get; set; }
    public int BankId { get; set; }
    public decimal Balance { get; set; }
    public AccountType AccountType { get; set; }
    public Enterprise EmployerEnterprise { get; set; }
}