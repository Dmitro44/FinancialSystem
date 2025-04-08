using FinancialSystem.Application.DTOs;
using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Application.Interfaces;

public interface ITransferService
{
    Task CreateTransferAsync(TransferDto dto);
    Task<(bool success, string message, int bankId)> CancelTransferAsync(int transferId);
    Task<IEnumerable<Transfer>> FetchTransfersByBankAsync(int bankId);
}