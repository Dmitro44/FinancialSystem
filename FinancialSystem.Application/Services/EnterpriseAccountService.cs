using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Interfaces;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialSystem.Application.Services;

public class EnterpriseAccountService : IEnterpriseAccountService
{
    private readonly IEnterpriseRepository _enterpriseRepository;
    private readonly IBankRepository _bankRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ILogger<EnterpriseAccountService> _logger;

    public EnterpriseAccountService(IEnterpriseRepository enterpriseRepository, IBankRepository bankRepository, IAccountRepository accountRepository, ILogger<EnterpriseAccountService> logger)
    {
        _enterpriseRepository = enterpriseRepository;
        _bankRepository = bankRepository;
        _accountRepository = accountRepository;
        _logger = logger;
    }

    public async Task CreateEnterpriseAccountAsync(EnterpriseAccountDto enterpriseAccountDto)
    {
        _logger.LogInformation("Starting creation of enterprise account for enterprise {EnterpriseId} in bank {BankId} with initial balance {Balance}",
            enterpriseAccountDto.EnterpriseId, enterpriseAccountDto.BankId, enterpriseAccountDto.Balance);
        
        var enterprise = await _enterpriseRepository.GetByIdAsync(enterpriseAccountDto.EnterpriseId);
        if (enterprise == null)
        {
            _logger.LogWarning("Failed to create enterprise account: enterprise with Id {EnterpriseId} not found",
                enterpriseAccountDto.EnterpriseId);
            throw new ApplicationException($"Enterprise with id: {enterpriseAccountDto.EnterpriseId} does not exist.");
        }
        
        var bank = await _bankRepository.GetByIdAsync(enterpriseAccountDto.BankId);
        if (bank == null)
        {
            _logger.LogWarning("Failed to create enterprise account: bank with Id {BankId} not found",
                enterpriseAccountDto.BankId);
            throw new ApplicationException($"Bank with id: {enterpriseAccountDto.BankId} does not exist.");
        }

        var enterpriseAccount = new EnterpriseAccount(enterprise, enterpriseAccountDto.Balance, bank);

        await _accountRepository.AddAsync(enterpriseAccount);
        
        _logger.LogInformation("Enterprise account successfully created for enterprise {EnterpriseId} in bank {BankId} with ID {AccountId}",
            enterpriseAccountDto.EnterpriseId, enterpriseAccountDto.BankId, enterpriseAccount.Id);
    }

    public async Task<IEnumerable<EnterpriseAccount>> FetchEnterpriseAccountsByBankAsync(int enterpriseId, int bankId)
    {
        _logger.LogInformation("Fetching enterprise accounts for enterprise {EnterpriseId} in bank {BankId}",
            enterpriseId, bankId);
        
        try
        {
            var accounts = await _enterpriseRepository.GetEnterpriseAccountsByBankAsync(enterpriseId, bankId);
            
            _logger.LogInformation("Successfully retrieved enterprise accounts for enterprise {EnterpriseId} in bank {BankId}",
                enterpriseId, bankId);
            
            return accounts;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching enterprise accounts for enterprise {EnterpriseId} in bank {BankId}", 
                enterpriseId, bankId);
            throw;
        }
    }
}