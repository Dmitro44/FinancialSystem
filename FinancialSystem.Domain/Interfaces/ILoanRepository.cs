using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;

namespace FinancialSystem.Domain.Interfaces;

public interface ILoanRepository
{
    Task<Loan?> GetByIdAsync(int loanId);
    Task AddAsync(Loan loan);
    Task UpdateAsync(Loan loan);
    Task UpdateStatusAsync(int loanId, RequestStatus status);
    Task DeleteAsync(int loanId);
    Task<IEnumerable<Loan>> GetUserLoansByBankAsync(int userId, int bankId);
    Task<IEnumerable<Loan>> GetLoansByBankAsync(int bankId);
}