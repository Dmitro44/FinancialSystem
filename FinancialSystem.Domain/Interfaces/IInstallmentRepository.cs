using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;

namespace FinancialSystem.Domain.Interfaces;

public interface IInstallmentRepository
{
    Task<Installment?> GetByIdAsync(int loanId);
    Task AddAsync(Installment installment);
    Task UpdateAsync(Installment installment);
    Task UpdateStatusAsync(int installmentId, RequestStatus newStatus);
    Task DeleteAsync(int installmentId);
    Task<IEnumerable<Installment>> GetUserInstallmentsByBankAsync(int userId, int bankId);
    Task<IEnumerable<Installment>> GetInstallmentsByBankAsync(int bankId);
}