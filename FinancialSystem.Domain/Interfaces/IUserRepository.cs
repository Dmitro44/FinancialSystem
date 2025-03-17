using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int userId);
    Task<User?> GetByEmailAsync(string userEmail);
    Task<bool> IsUserExists(string userIdentificationNumber);
    Task<User?> GetByIdWithRolesAsync(int userId);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int userId);
    Task<Role?> GetRoleInBankAsync(int userId, int bankId);
}