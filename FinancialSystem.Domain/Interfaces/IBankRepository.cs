using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Domain.Interfaces;

public interface IBankRepository
{
    Task<Bank?> GetByIdAsync(int bankId);
    Task AddAsync(Bank bank);
    Task UpdateAsync(Bank bank);
    Task DeleteAsync(int bankId);
    Task AddUserToBankAsync(UserBankRole userBankRole);
    Task<List<Bank>> GetAllBanksAsync();
}