using FinancialSystem.Application.DTOs;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Interfaces;

namespace FinancialSystem.Application.Services;

public class LoanService
{
    private readonly IFinancialCalculator _calculator;
    private readonly IUserRepository _userRepository;
    private readonly IBankRepository _bankRepository;

    public LoanService(IFinancialCalculator calculator, IUserRepository userRepository, IBankRepository bankRepository)
    {
        _calculator = calculator;
        _userRepository = userRepository;
        _bankRepository = bankRepository;
    }

    public async Task<Loan> CreateLoanAsync(LoanDto dto)
    {
        var totalAmount = _calculator.CalculateTotalAmount(dto.Amount, dto.InterestRate);
        var monthlyPayment = _calculator.CalculateMonthlyPayment(dto.Amount, dto.TermInMonths);

        var bank = await _bankRepository.GetByIdAsync(dto.BankId);
        var borrower = await _userRepository.GetByIdAsync(dto.UserId);
        if (borrower == null)
        {
            throw new ApplicationException($"User with id: {dto.UserId} does not exist.");
        }

        if (bank == null)
        {
            throw new ApplicationException($"Bank with id: {dto.BankId} does not exist.");
        }
        
        return new Loan(borrower, bank, dto.Amount, dto.TermInMonths, dto.InterestRate, totalAmount, monthlyPayment, dto.StartDate);
    }
}