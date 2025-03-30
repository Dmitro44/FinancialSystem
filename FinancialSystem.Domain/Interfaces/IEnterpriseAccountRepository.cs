using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Domain.Interfaces;

public interface IEnterpriseAccountRepository
{
    Task<EnterpriseAccount?> GetByIdAsync(int enterpriseAccountId);
    Task AddAsync(EnterpriseAccount entepriseAccount);
    Task UpdateAsync(EnterpriseAccount enterpriseAccount);
    Task DeleteAsync(int entepriseAccountId);
}