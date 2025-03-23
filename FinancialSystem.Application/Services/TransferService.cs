using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Interfaces;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;
using FinancialSystem.Domain.Interfaces;

namespace FinancialSystem.Application.Services;

public class TransferService : ITransferService
{
    private readonly ITransferRepository _transferRepository;
    private readonly IAccountRepository _accountRepository;

    public TransferService(ITransferRepository transferRepository, IAccountRepository accountRepository)
    {
        _transferRepository = transferRepository;
        _accountRepository = accountRepository;
    }

    public async Task CreateTransferAsync(TransferDto dto)
    {
        var senderAccount = await _accountRepository.GetByIdAsync(dto.SenderId)
                            ?? throw new ApplicationException($"Account {dto.SenderId} does not exist");

        var receiverAccount = await _accountRepository.GetByIdAsync(dto.ReceiverId)
                              ?? throw new ApplicationException($"Account {dto.ReceiverId} does not exist");

        if (senderAccount.Balance < dto.Amount)
        {
            throw new InvalidOperationException("Insufficient balance");
        }

        senderAccount.Withdraw(dto.Amount);
        receiverAccount.Deposit(dto.Amount);
        
        await _accountRepository.UpdateAsync(senderAccount);
        await _accountRepository.UpdateAsync(receiverAccount);

        var transfer = new Transfer(senderAccount, receiverAccount, dto.Amount, dto.Status);

        await _transferRepository.AddAsync(transfer);
    }

    public async Task<IEnumerable<Transfer>> FetchTransfersByBankAsync(int bankId)
    {
        return await _transferRepository.GetTransfersByBankAsync(bankId);
    }

    public async Task<Transfer?> GetByIdAsync(int transferId)
    {
        return await _transferRepository.GetByIdAsync(transferId);
    }

    public async Task UpdateStatusAsync(int transferId, TransferStatus status)
    {
        await _transferRepository.UdpateStatusAsync(transferId, status);
    }
}