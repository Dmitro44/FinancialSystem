using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int userId);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int userId);
}