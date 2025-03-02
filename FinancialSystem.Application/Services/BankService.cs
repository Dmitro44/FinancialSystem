using FinancialSystem.Application.DTOs;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Interfaces;

namespace FinancialSystem.Application.Services;

public class BankService
{
    private readonly IBankRepository _bankRepository;
    private readonly IUserRepository _userRepository;

    public BankService(IBankRepository bankRepository, IUserRepository userRepository)
    {
        _bankRepository = bankRepository;
        _userRepository = userRepository;
    }

    public async Task AssignUserToBank(UserBankDto dto)
    {
        var user = await _userRepository.GetByIdAsync(dto.UserId);
        var bank = await _bankRepository.GetByIdAsync(dto.BankId);

        if (user == null)
        {
            throw new ArgumentNullException($"User with id: {dto.UserId} not found");
        }

        if (bank == null)
        {
            throw new ArgumentNullException($"Bank with id: {dto.BankId} not found");
        }
        
        var userBankRole = new UserBankRole(user, bank, dto.Role);
        await _bankRepository.AddUserToBankAsync(userBankRole);
    }

    public Bank CreateBank(BankDto dto)
    {
        return new Bank(dto.Name, dto.Bic, dto.Address);
    }
}