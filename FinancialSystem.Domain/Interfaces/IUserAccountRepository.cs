using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Domain.Interfaces;

public interface IUserAccountRepository
{
    Task<UserAccount?> GetByIdAsync(int accountId);
    Task AddAsync(UserAccount userAccount);
    Task UpdateAsync(UserAccount userAccount);
    Task DeleteAsync(int accountId);
    Task<IEnumerable<UserAccount>> GetUserAccountsByBankAsync(int userId, int bankId);
}