using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Domain.Interfaces;

public interface ILoanRepository
{
    Task<Loan?> GetByIdAsync(int loanId);
    Task AddAsync(Loan loan);
    Task UpdateAsync(Loan loan);
    Task DeleteAsync(int loanId);
    Task<IEnumerable<Loan>> GetLoansByUserIdAsync(int userId);
}