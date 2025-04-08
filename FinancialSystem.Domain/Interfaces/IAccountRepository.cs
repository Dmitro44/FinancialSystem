using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Domain.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(int accountId);
    Task AddAsync(Account account);
    Task UpdateAsync(Account account);
    Task DeleteAsync(int accountId);
    Task<UserAccount?> GetUserAccountByIdAsync(int accountId);
    Task<EnterpriseAccount?> GetEnterpriseAccountByIdAsync(int accountId);
    Task<IEnumerable<UserAccount>> GetUserAccountsByBankAsync(int userId, int bankId);
    Task<UserAccount?> GetInactiveAccountBySalaryProjectAsync(int userId, int salaryProjectId);
}