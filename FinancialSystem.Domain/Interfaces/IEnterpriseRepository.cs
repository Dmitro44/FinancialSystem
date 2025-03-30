using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Domain.Interfaces;

public interface IEnterpriseRepository
{
    Task<Enterprise?> GetByIdAsync(int enterpriseId);
    Task AddAsync(Enterprise enterprise);
    Task UpdateAsync(Enterprise enterprise);
    Task DeleteAsync(int enterpriseId);
    Task<Enterprise?> GetUserEnterpriseByBankAsync(int currentUserId, int bankId);
    Task<IEnumerable<EnterpriseAccount>> GetEnterpriseAccountsByBankAsync(int enterpriseId, int bankId);
}