using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Interfaces;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;
using FinancialSystem.Domain.Interfaces;
using Microsoft.VisualBasic;

namespace FinancialSystem.Application.Services;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _loanRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBankRepository _bankRepository;

    public LoanService(IUserRepository userRepository, IBankRepository bankRepository,
        ILoanRepository loanRepository)
    {
        _userRepository = userRepository;
        _bankRepository = bankRepository;
        _loanRepository = loanRepository;
    }

    public async Task CreateLoanAsync(LoanDto dto)
    {
        var bank = await _bankRepository.GetByIdAsync(dto.BankId)
            ?? throw new ApplicationException($"Bank with id: {dto.BankId} does not exist.");
            
        var borrower = await _userRepository.GetByIdAsync(dto.UserId)
            ?? throw new ApplicationException($"User with id: {dto.UserId} does not exist.");
        
        var loan = new Loan(borrower, bank, dto.Amount, dto.TermInMonths, dto.InterestRate, dto.TotalAmount, dto.MonthlyPayment);
        
        await _loanRepository.AddAsync(loan);
    }

    public async Task<IEnumerable<Loan>> FetchUserLoansByBankAsync(int userId, int bankId)
    {
        return await _loanRepository.GetUserLoansByBankAsync(userId, bankId);
    }

    public async Task<IEnumerable<Loan>> FetchLoansByBankAsync(int bankId)
    {
        return await _loanRepository.GetLoansByBankAsync(bankId);
    }

    public async Task<Loan?> GetLoanByIdAsync(int loanId)
    {
        var loan = await _loanRepository.GetByIdAsync(loanId)
            ?? throw new InvalidOperationException($"Loan with id: {loanId} does not exist.");

        return loan;
    }

    public async Task UpdateLoanAsync(Loan loan)
    {
        await _loanRepository.UpdateAsync(loan);
    }

    public async Task UpdateStatusAsync(int loanId, RequestStatus newStatus)
    {
        await _loanRepository.UpdateStatusAsync(loanId, newStatus);
    }
}