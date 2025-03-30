using FinancialSystem.Application.DTOs;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;

namespace FinancialSystem.Application.Interfaces;

public interface ILoanService
{
    Task CreateLoanAsync(LoanDto dto);
    Task<IEnumerable<Loan>> FetchUserLoansByBankAsync(int userId, int bankId);
    Task<IEnumerable<Loan>> FetchLoansByBankAsync(int bankId);
    // Task<Loan?> GetLoanByIdAsync(int loanId);
    // Task UpdateLoanAsync(Loan loan);
    Task UpdateStatusAsync(int loanId, RequestStatus newStatus);
}