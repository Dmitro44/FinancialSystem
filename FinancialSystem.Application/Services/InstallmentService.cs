using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Interfaces;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;
using FinancialSystem.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialSystem.Application.Services;

public class InstallmentService : IInstallmentService
{
    private readonly IUserRepository _userRepository;
    
    private readonly IUserAccountService _userAccountService;
    private readonly IAccountRepository _accountRepository;
    private readonly ITransferService _transferService;
    private readonly IBankRepository _bankRepository;
    private readonly IInstallmentRepository _installmentRepository;
    private readonly ILogger<InstallmentService> _logger;

    public InstallmentService(IUserRepository userRepository, IBankRepository bankRepository,
        IInstallmentRepository installmentRepository, ILogger<InstallmentService> logger, IUserAccountService userAccountService, ITransferService transferService, IAccountRepository accountRepository)
    {
        _userRepository = userRepository;
        _bankRepository = bankRepository;
        _installmentRepository = installmentRepository;
        _logger = logger;
        _userAccountService = userAccountService;
        _transferService = transferService;
        _accountRepository = accountRepository;
    }
    
    public async Task CreateInstallmentAsync(InstallmentDto dto)
    {
        _logger.LogInformation("Starting creation of installment for user {UserId} in bank {BankId} with amount {Amount} for {Term} months",
            dto.UserId, dto.BankId, dto.Amount, dto.TermInMonths);
        
        var monthlyPayment = FinancialCalculator.CalculateMonthlyPayment(dto.Amount, dto.TermInMonths);

        var bank = await _bankRepository.GetByIdAsync(dto.BankId);
        var payer = await _userRepository.GetByIdAsync(dto.UserId);
        if (payer == null)
        {
            _logger.LogWarning("Failed to create installment: user with ID {UserId} not found",
                dto.UserId);
            throw new ApplicationException($"User with id: {dto.UserId} does not exist.");
        }

        if (bank == null)
        {
            _logger.LogWarning("Failed to create installment: bank with ID {BankId} not found",
                dto.BankId);
            throw new ApplicationException($"Bank with id: {dto.BankId} does not exist.");
        }
        
        var installment = new Installment(payer, bank, dto.Amount, dto.TermInMonths, monthlyPayment, dto.StartDate, dto.DestinationAccountId);
        
        await _installmentRepository.AddAsync(installment);
        
        _logger.LogInformation("Installment successfully created for user {UserId} in bank {BankId}",
            dto.UserId, dto.BankId);
    }

    public async Task AddInstallmentAccount(int installmentId)
    {
        var installment = await _installmentRepository.GetByIdAsync(installmentId);
        if (installment == null)
        {
            _logger.LogWarning("Failed to create installment: installment with ID {InstallmentId} not found",
                installmentId);
            throw new ApplicationException($"Installment with ID {installmentId} not found");
        }

        var installmentAccountDto = new UserAccountDto
        {
            OwnerId = installment.PayerId,
            BankId = installment.BankId,
            Balance = 0,
            AccountType = AccountType.Credit
        };

        var installmentAccount = await _userAccountService.CreateUserAccountAsync(installmentAccountDto);
        if (installmentAccount == null)
        {
            _logger.LogWarning("Failed to create installment account");
            throw new ApplicationException("Installment account could not be created");
        }

        installment.AddInstallmentAccount(installmentAccount);
        await _installmentRepository.UpdateAsync(installment);
    }

    public async Task SendInstallmentAmount(int installmentId)
    {
        var installment = await _installmentRepository.GetByIdAsync(installmentId);
        if (installment == null)
        {
            _logger.LogWarning("Failed to send installment amount: installment with ID {InstallmentId} not found",
                installmentId);
            throw new ApplicationException($"Installment with ID {installmentId} not found");
        }

        var transferDto = new TransferDto
        {
            SenderId = installment.InstallmentAccountId.Value,
            ReceiverId = installment.DestinationAccountId,
            Amount = installment.Amount,
            IsSenderCreditAccount = true
        };

        await _transferService.CreateTransferAsync(transferDto);
    }

    public async Task<IEnumerable<Installment>> FetchUserInstallmentsByBankAsync(int userId, int bankId)
    {
        _logger.LogInformation("Fetching installments for user {UserId} in bank {BankId}",
            userId, bankId);

        try
        {
            var installments = await _installmentRepository.GetUserInstallmentsByBankAsync(userId, bankId);
            
            _logger.LogInformation("Successfully fetched installments for user {UserId} in bank {BankId}",
                userId, bankId);
            
            return installments;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching installments for user {UserId} in bank {BankId}",
                userId, bankId);
            throw;
        }
    }

    public async Task<IEnumerable<Installment>> FetchInstallmentsByBankAsync(int bankId)
    {
        _logger.LogInformation("Fetching all installments in bank {BankId}", bankId);

        try
        {
            var installments = await _installmentRepository.GetInstallmentsByBankAsync(bankId);
            
            _logger.LogInformation("Successfully fetched all installments for bank {BankId}", bankId);
            
            return installments;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching installments for bank {BankId}", bankId);
            throw;
        }
    }

    public async Task UpdateStatusAsync(int installmentId, RequestStatus newStatus)
    {
        _logger.LogInformation("Updating status of installment {InstallmentId} to {NewStatus}", installmentId, newStatus);

        try
        {
            await _installmentRepository.UpdateStatusAsync(installmentId, newStatus);
            
            _logger.LogInformation("Successfully updated status of installment {InstallmentId} to {NewStatus}", installmentId, newStatus);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating status of installment {InstallmentId} to {NewStatus}", installmentId, newStatus);
            throw;
        }
    }

    public async Task<bool> RevertInstallmentCreationAsync(int installmentId)
    {
        _logger.LogInformation("Reverting installment creation for installment {InstallmentId}",
            installmentId);

        var installment = await _installmentRepository.GetByIdAsync(installmentId);
        if (installment == null || installment.Status != RequestStatus.Approved)
        {
            _logger.LogWarning("Cannot revert installment creation: installment {InstallmentId} not found or not approved",
                installmentId);
            return false;
        }

        try
        {
            await _installmentRepository.UpdateStatusAsync(installmentId, RequestStatus.Rejected);

            if (installment.InstallmentAccountId.HasValue)
            {
                var installmentAccount = await _accountRepository.GetByIdAsync(installment.InstallmentAccountId.Value);
                if (installmentAccount != null)
                {
                    var destinationAccount = await _accountRepository.GetByIdAsync(installment.DestinationAccountId);
                    if (destinationAccount != null && destinationAccount.Balance >= installment.Amount)
                    {
                        await _transferService.CreateTransferAsync(new TransferDto
                        {
                            SenderId = installment.DestinationAccountId,
                            ReceiverId = installment.InstallmentAccountId.Value,
                            Amount = installment.Amount,
                            IsSenderCreditAccount = true
                        });
                    }
                    
                    installmentAccount.Deactivate();
                    await _accountRepository.UpdateAsync(installmentAccount);
                }
            }
        
            _logger.LogInformation("Successfully reverted installment creation for installment {InstallmentId}",
                installmentId);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reverting installment creation for installment {InstallmentId}",
                installmentId);
            return false;
        }
    }

    public async Task<bool> RestoreInstallmentCreationAsync(int installmentId)
    {
        _logger.LogInformation("Restoration installment creation for installment {InstallmentId}",
            installmentId);
        
        var installment = await _installmentRepository.GetByIdAsync(installmentId);
        if (installment == null)
        {
            _logger.LogWarning("Cannot restore installment creation: installment {InstallmentId} not found",
                installmentId);
            return false;
        }

        try
        {
            await _installmentRepository.UpdateStatusAsync(installmentId, RequestStatus.Approved);

            if (installment.InstallmentAccountId.HasValue)
            {
                var installmentAccount = await _accountRepository.GetByIdAsync(installment.InstallmentAccountId.Value);
                if (installmentAccount != null)
                {
                    installmentAccount.Activate();
                    await _accountRepository.UpdateAsync(installmentAccount);

                    await _transferService.CreateTransferAsync(new TransferDto
                    {
                        SenderId = installment.InstallmentAccountId.Value,
                        ReceiverId = installment.DestinationAccountId,
                        Amount = installment.Amount,
                        Type = TransferType.Regular,
                        IsSenderCreditAccount = true
                    });
                }
            }
        
            _logger.LogInformation("Successfully restored installment creation for installment {InstallmentId}",
                installmentId);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error restoring installment creation for installment {InstallmentId}",
                installmentId);
            return false;
        }
    }
}