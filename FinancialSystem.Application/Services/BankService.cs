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

    public async Task RegisterUserToBankAsync(UserBankDto dto)
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

    public async Task<(List<UserBankRole>, List<Bank>)> GetUserBanksAsync(int userId)
    {
        var user = await _userRepository.GetByIdWithRolesAsync(userId);
        if (user == null)
            throw new InvalidOperationException("User not found");

        var registeredBanks = user.UserBankRoles.ToList();

        var allBanks = await _bankRepository.GetAllBanksAsync();
        
        var otherBanks = allBanks.Where(b => registeredBanks.All(r => r.BankId != b.Id)).ToList();
        
        return (registeredBanks, otherBanks);
    }
}