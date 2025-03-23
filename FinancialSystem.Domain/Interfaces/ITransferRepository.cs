using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;

namespace FinancialSystem.Domain.Interfaces;

public interface ITransferRepository
{
    Task<Transfer?> GetByIdAsync(int transferId);
    Task AddAsync(Transfer transfer);
    Task DeleteAsync(int transferId);
    Task<IEnumerable<Transfer>> GetTransfersByBankAsync(int bankId);
    Task UdpateStatusAsync(int transferId, TransferStatus status);
}