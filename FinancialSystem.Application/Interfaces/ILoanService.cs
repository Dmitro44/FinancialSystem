using FinancialSystem.Application.DTOs;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;

namespace FinancialSystem.Application.Interfaces;

public interface ILoanService
{
    Task CreateLoanAsync(LoanDto dto);
    Task AddLoanAccount(int loanId);
    Task SendLoanAmount(int loanId);
    Task<IEnumerable<Loan>> FetchUserLoansByBankAsync(int userId, int bankId);
    Task<IEnumerable<Loan>> FetchLoansByBankAsync(int bankId);
    Task UpdateStatusAsync(int loanId, RequestStatus newStatus);
    Task<bool> RevertLoanCreationAsync(int logEntityId);
    Task<bool> RestoreLoanCreationAsync(int loanId);
}