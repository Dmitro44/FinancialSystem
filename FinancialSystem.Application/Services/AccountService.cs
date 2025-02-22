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
        var user = await _userRepository.GetByIdAsync(dto.UserId);
        var bank = await _bankRepository.GetByIdAsync(dto.BankId);
        if (user == null)
        {
            throw new ApplicationException($"User with id: {dto.UserId} does not exist.");
        }

        if (bank == null)
        {
            throw new ApplicationException($"Bank with id: {dto.BankId} does not exist.");
        }
        
        var account = new Account(user, dto.Balance, bank);
        
        await _accountRepository.AddAsync(account);
        
        return account;
    }
}