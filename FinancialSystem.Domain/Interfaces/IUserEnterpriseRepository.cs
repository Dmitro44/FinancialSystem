using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Domain.Interfaces;

public interface IUserEnterpriseRepository
{
    Task<UserEnterprise?> GetByIdAsync(int id);
    Task AddAsync(UserEnterprise userEnterprise);
    Task UpdateAsync(UserEnterprise userEnterprise);
    Task DeleteAsync(int userEnterpriseId);
    
    Task<UserEnterprise?> GetByUserAndEnterpriseId(int userId, int enterpriseId);
    Task<IEnumerable<UserEnterprise>> GetByUserIdAsync(int userId);
}