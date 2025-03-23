using FinancialSystem.Application.DTOs;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;

namespace FinancialSystem.Application.Interfaces;

public interface ITransferService
{
    Task CreateTransferAsync(TransferDto dto);
    Task<IEnumerable<Transfer>> FetchTransfersByBankAsync(int bankId);
    Task<Transfer?> GetByIdAsync(int transferId);
    Task UpdateStatusAsync(int transferId, TransferStatus status);
}