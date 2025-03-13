using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Domain.Interfaces;

public interface IInstallmentRepository
{
    Task<Installment?> GetByIdAsync(int loanId);
    Task AddAsync(Installment installment);
    Task UpdateAsync(Installment installment);
    Task DeleteAsync(int installmentId);
    Task<IEnumerable<Installment>> GetInstallmentsByUserIdAsync(int userId);
}