using FinancialSystem.Application.DTOs;
using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Application.Interfaces;

public interface IBankService
{
    Task RegisterUserToBankAsync(UserBankDto dto);
    Task<(List<UserBankRole>, List<Bank>)> GetUserBanksAsync(int userId);
    Task<Bank?> GetBankByIdAsync(int bankId);
    Task<IEnumerable<UserAccount>> RetrieveUserAccountsByBankAsync(int userId, int bankId);
    Task<IEnumerable<Loan>> RetrieveUserLoansByBankAsync(int userId, int bankId);
    Task<IEnumerable<Installment>> RetrieveUserInstallmentsByBankAsync(int userId, int bankId);
    Task<IEnumerable<Loan>> RetrieveLoansByBankAsync(int bankId);
    Task<IEnumerable<Installment>> RetrieveInstallmentsByBankAsync(int bankId);
    Task<IEnumerable<Transfer>> RetrieveTransfersByBankAsync(int bankId);
    Task CreateAccountAsync(UserAccountDto dto);
    Task CreateLoanAsync(LoanDto dto);
    Task CreateInstallmentAsync(InstallmentDto dto);
    Task CreateTransferAsync(TransferDto transferDto);
    Task<IEnumerable<SalaryProject>> RetrieveAllSalaryProjectsByBankAsync(int bankId);
}