using FinancialSystem.Application.DTOs;
using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Application.Interfaces;

public interface IAccountService
{
    Task CreateAccountAsync(AccountDto accountDto);
    Task<IEnumerable<Account>> FetchUserAccountsByBankAsync(int userId, int bankId);
}