using FinancialSystem.Application.DTOs;
using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Application.Interfaces;

public interface IUserAccountService
{
    Task<UserAccount?> CreateUserAccountAsync(UserAccountDto userAccountDto);
    Task<IEnumerable<UserAccount>> FetchUserAccountsByBankAsync(int userId, int bankId);
}