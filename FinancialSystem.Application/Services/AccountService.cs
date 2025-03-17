using FinancialSystem.Application.DTOs;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Interfaces;

namespace FinancialSystem.Application.Services;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBankRepository _bankRepository;

    public AccountService(IUserRepository userRepository, IBankRepository bankRepository,
        IAccountRepository accountRepository)
    {
        _userRepository = userRepository;
        _bankRepository = bankRepository;
        _accountRepository = accountRepository;
    }

    public async Task<Account> CreateAccountAsync(AccountDto dto)
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
        
        var account = new Account(user, dto.Balance, bank);
        
        await _accountRepository.AddAsync(account);
        
        return account;
    }

    public async Task<IEnumerable<Account>> FetchUserAccountsByBankAsync(int userId, int bankId)
    {
        return await _accountRepository.GetUserAccountsByBankAsync(userId, bankId);
    }
}