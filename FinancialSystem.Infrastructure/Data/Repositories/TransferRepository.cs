using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;
using FinancialSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialSystem.Infrastructure.Data.Repositories;

public class TransferRepository : ITransferRepository
{
    private readonly ApplicationDbContext _context;

    public TransferRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Transfer?> GetByIdAsync(int transferId)
    {
        return await _context.Transfers.FindAsync(transferId);
    }

    public async Task AddAsync(Transfer transfer)
    {
        await _context.Transfers.AddAsync(transfer);
        await _context.SaveChangesAsync();
    }
    
    public async Task UdpateStatusAsync(int transferId, TransferStatus status)
    {
        var transferToUpdate = await _context.Transfers.FindAsync(transferId);
        if (transferToUpdate == null)
            throw new ArgumentNullException(nameof(transferToUpdate));
        
        transferToUpdate.SetStatus(status);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int transferId)
    {
        var transferToDelete = await _context.Transfers.FindAsync(transferId);
        if (transferToDelete == null)
        {
            throw new ArgumentNullException(nameof(transferToDelete));
        }
        
        _context.Transfers.Remove(transferToDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Transfer>> GetTransfersByBankAsync(int bankId)
    {
        return await _context.Transfers
            .Where(t => t.Sender.BankId == bankId)
            .ToListAsync();
    }
}