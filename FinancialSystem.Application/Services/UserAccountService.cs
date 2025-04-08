using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Interfaces;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;
using FinancialSystem.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialSystem.Application.Services;

public class UserAccountService : IUserAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBankRepository _bankRepository;
    private readonly ILogger<UserAccountService> _logger;

    public UserAccountService(IUserRepository userRepository, IBankRepository bankRepository,
        IAccountRepository accountRepository, ILogger<UserAccountService> logger)
    {
        _userRepository = userRepository;
        _bankRepository = bankRepository;
        _accountRepository = accountRepository;
        _logger = logger;
    }

    public async Task<UserAccount?> CreateUserAccountAsync(UserAccountDto dto)
    {
        _logger.LogInformation("Starting creation of user account for user {UserId} in bank {BankId} with initial balance {Balance} and type {AccountType}",
            dto.OwnerId, dto.BankId, dto.Balance, dto.AccountType);

        try
        {
            var user = await _userRepository.GetByIdAsync(dto.OwnerId);
            var bank = await _bankRepository.GetByIdAsync(dto.BankId);
            if (user == null)
            {
                _logger.LogWarning("Failed to create user account: user with ID {UserId} not found", 
                    dto.OwnerId);
                throw new ApplicationException($"User with id: {dto.OwnerId} does not exist.");
            }

            if (bank == null)
            {
                _logger.LogWarning("Failed to create user account: bank with ID {BankId} not found", 
                    dto.BankId);
                throw new ApplicationException($"Bank with id: {dto.BankId} does not exist.");
            }
        
            var account = new UserAccount(user, dto.Balance, bank, dto.AccountType);

            if (dto.AccountType == AccountType.Salary)
            {
                account.SetEmployer(dto.EmployerEnterprise);
            }
        
            await _accountRepository.AddAsync(account);

            _logger.LogInformation("User account successfully created for user {UserId} in bank {BankId} with ID {AccountId}", 
                dto.OwnerId, dto.BankId, account.Id);
        
            return account;
        }
        catch (Exception e) when (!(e is ApplicationException))
        {
            _logger.LogError(e, "Error creating user account for user {UserId} in bank {BankId}", 
                dto.OwnerId, dto.BankId);
            throw;
        }
    }

    public async Task<IEnumerable<UserAccount>> FetchUserAccountsByBankAsync(int userId, int bankId)
    {
        _logger.LogInformation("Fetching user accounts for user {UserId} in bank {BankId}", 
            userId, bankId);
        
        try
        {
            var accounts =  await _accountRepository.GetUserAccountsByBankAsync(userId, bankId);
            
            _logger.LogInformation("Successfully fetched accounts for user {UserId} in bank {BankId}", 
                userId, bankId);
            
            return accounts;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching accounts for user {UserId} in bank {BankId}", 
                userId, bankId);
            throw;
        }
    }
}