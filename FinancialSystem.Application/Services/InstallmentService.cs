using FinancialSystem.Application.DTOs;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Interfaces;

namespace FinancialSystem.Application.Services;

public class InstallmentService
{
    private readonly IFinancialCalculator _calculator;
    private readonly IUserRepository _userRepository;
    private readonly IBankRepository _bankRepository;

    public InstallmentService(IFinancialCalculator calculator, IUserRepository userRepository,
        IBankRepository bankRepository)
    {
        _calculator = calculator;
        _userRepository = userRepository;
        _bankRepository = bankRepository;
    }

    public async Task<Installment> CreateInstallmentAsync(InstallmentDto dto)
    {
        var monthlyPayment = _calculator.CalculateMonthlyPayment(dto.Amount, dto.TermInMonths);

        var bank = await _bankRepository.GetByIdAsync(dto.BankId);
        var payer = await _userRepository.GetByIdAsync(dto.UserId);
        if (payer == null)
        {
            throw new ApplicationException($"User with id: {dto.UserId} does not exist.");
        }

        if (bank == null)
        {
            throw new ApplicationException($"Bank with id: {dto.BankId} does not exist.");
        }
        
        return new Installment(payer, bank, dto.Amount, dto.TermInMonths, dto.InterestRate, monthlyPayment, dto.StartDate);
        
        
    }
}