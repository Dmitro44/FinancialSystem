using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Interfaces;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Interfaces;

namespace FinancialSystem.Application.Services;

public class UserAccountService : IUserAccountService
{
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBankRepository _bankRepository;

    public UserAccountService(IUserRepository userRepository, IBankRepository bankRepository,
        IUserAccountRepository userAccountRepository)
    {
        _userRepository = userRepository;
        _bankRepository = bankRepository;
        _userAccountRepository = userAccountRepository;
    }

    public async Task CreateAccountAsync(UserAccountDto dto)
    {
        var user = await _userRepository.GetByIdAsync(dto.OwnerId);
        var bank = await _bankRepository.GetByIdAsync(dto.BankId);
        if (user == null)
        {
            throw new ApplicationException($"User with id: {dto.OwnerId} does not exist.");
        }

        if (bank == null)
        {
            throw new ApplicationException($"Bank with id: {dto.BankId} does not exist.");
        }
        
        var account = new UserAccount(user, dto.Balance, bank);
        
        await _userAccountRepository.AddAsync(account);
    }

    public async Task<IEnumerable<UserAccount>> FetchUserAccountsByBankAsync(int userId, int bankId)
    {
        return await _userAccountRepository.GetUserAccountsByBankAsync(userId, bankId);
    }
}