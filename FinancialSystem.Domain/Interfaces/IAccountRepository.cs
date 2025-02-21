using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Domain.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(int accountId);
    Task AddAsync(Account account);
    Task UpdateAsync(Account account);
    Task DeleteAsync(int accountId);
}