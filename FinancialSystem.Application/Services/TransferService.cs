using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Interfaces;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;
using FinancialSystem.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialSystem.Application.Services;

public class TransferService : ITransferService
{
    private readonly ITransferRepository _transferRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ILogger<TransferService> _logger;

    public TransferService(ITransferRepository transferRepository, IAccountRepository accountRepository, ILogger<TransferService> logger)
    {
        _transferRepository = transferRepository;
        _accountRepository = accountRepository;
        _logger = logger;
    }

    public async Task CreateTransferAsync(TransferDto dto)
    {
        _logger.LogInformation("Starting creation of transfer from account {SenderId} to account {ReceiverId} with amount {Amount} and type {AccountType}",
            dto.SenderId, dto.ReceiverId, dto.Amount, dto.Type);

        try
        {
            var senderAccount = await _accountRepository.GetByIdAsync(dto.SenderId);
            if (senderAccount == null)
            {
                _logger.LogWarning("Failed to create transfer: sender account {SenderId} not found", 
                    dto.SenderId);
                throw new ApplicationException($"User account {dto.SenderId} does not exist");
            }
            
            if (!senderAccount.IsActive)
            {
                _logger.LogWarning("Failed to create transfer: sender account {SenderId} is inactive",
                    dto.SenderId);
                throw new InvalidOperationException("Cannot transfer from inactive account");
            }

            var receiverAccount = await _accountRepository.GetByIdAsync(dto.ReceiverId);
            if (receiverAccount == null)
            {
                _logger.LogWarning("Failed to create transfer: receiver account {ReceiverId} not found",
                    dto.ReceiverId);
                throw new ApplicationException($"User account {dto.ReceiverId} does not exist");
            }

            if (!receiverAccount.IsActive)
            {
                _logger.LogWarning("Failed to create transfer: receiver account {ReceiverId} is inactive",
                    dto.ReceiverId);
                throw new InvalidOperationException("Cannot transfer to inactive account");
            }

            if (senderAccount.Balance < dto.Amount && !dto.IsSenderCreditAccount)
            {
                _logger.LogWarning("Insufficient balance for transfer: account {AccountId} has {Balance}, but {RequiredAmount} is needed", 
                    senderAccount.Id, senderAccount.Balance, dto.Amount);
                throw new InvalidOperationException("Insufficient balance");
            }

            senderAccount.Withdraw(dto.Amount);
            receiverAccount.Deposit(dto.Amount);
        
            await _accountRepository.UpdateAsync(senderAccount);
            await _accountRepository.UpdateAsync(receiverAccount);

            var transfer = new Transfer(senderAccount, receiverAccount, dto.Amount, dto.Status, dto.Type);

            await _transferRepository.AddAsync(transfer);
        
            _logger.LogInformation("Transfer {TransferId} successfully created from account {SenderId} to account {ReceiverId} with amount {Amount}", 
                transfer.Id, dto.SenderId, dto.ReceiverId, dto.Amount);
        }
        catch (Exception e) when (!(e is ApplicationException || e is InvalidOperationException))
        {
            _logger.LogError(e, "Error creating transfer from account {SenderId} to account {ReceiverId} with amount {Amount}", 
                dto.SenderId, dto.ReceiverId, dto.Amount);
            throw;
        }
    }

    public async Task<(bool success, string message, int bankId)> CancelTransferAsync(int transferId)
    {
        _logger.LogInformation("Starting cancellation of transfer {TransferId}", transferId);
        
        var transfer = await _transferRepository.GetByIdAsync(transferId);
        if (transfer == null)
        {
            _logger.LogWarning("Failed to cancel transfer: transfer {TransferId} not found", transferId);
            return (false, "Transfer not found", transferId);
        }

        var bankId = transfer.Sender.BankId;

        var transferDto = new TransferDto
        {
            SenderId = transfer.ReceiverId,
            ReceiverId = transfer.SenderId,
            Amount = transfer.Amount,
            Status = TransferStatus.Revert,
            Type = transfer.Type
        };
            
        try
        {
            await CreateTransferAsync(transferDto);
            await _transferRepository.UdpateStatusAsync(transferId, TransferStatus.Canceled);
        
            _logger.LogInformation("Transfer {TransferId} has been successfully canceled", transferId);
            return (true, "Transfer has been canceled", bankId);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Error during transfer cancellation for transfer {TransferId}: {ErrorMessage}", 
                transferId, e.Message);
            return (false, $"Error canceling transfer: {e.Message}", bankId);
        }
    }

    public async Task<IEnumerable<Transfer>> FetchTransfersByBankAsync(int bankId)
    {
        _logger.LogInformation("Fetching all transfers for bank {BankId}", bankId);
        
        try
        {
            var transfers = (await _transferRepository.GetTransfersByBankAsync(bankId)).ToList();
                
            _logger.LogInformation("Successfully retrieved all transfers for bank {BankId}", bankId);
                
            return transfers;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error retrieving transfers for bank {BankId}", bankId);
            throw;
        }
    }
}