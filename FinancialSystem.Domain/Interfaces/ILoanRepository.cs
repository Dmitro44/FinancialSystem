using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Domain.Interfaces;

public interface ILoanRepository
{
    Task<Loan?> GetByIdAsync(int loanId);
    Task AddAsync(Loan loan);
    Task UpdateAsync(Loan loan);
    Task UpdateStatusAsync(int loanId, LoanStatus status);
    Task DeleteAsync(int loanId);
    Task<IEnumerable<Loan>> GetUserLoansByBankAsync(int userId, int bankId);
    Task<IEnumerable<Loan>> GetLoansByBankAsync(int bankId);
}