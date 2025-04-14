using FinancialSystem.Domain.Interfaces;
using FinancialSystem.Domain.Operations;
using Microsoft.EntityFrameworkCore;

namespace FinancialSystem.Infrastructure.Data.Repositories;

public class OperationLogRepository : IOperationLogRepository
{
    private readonly ApplicationDbContext _context;

    public OperationLogRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<OperationLog?> GetByIdAsync(int logId)
    {
        return await _context.OperationLogs.FindAsync(logId);
    }

    public async Task AddAsync(OperationLog operationLog)
    {
        await _context.OperationLogs.AddAsync(operationLog);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(OperationLog operationLog)
    {
        var operationLogToUpdate = await _context.OperationLogs.FindAsync(operationLog.Id);
        if (operationLogToUpdate == null)
        {
            throw new ArgumentNullException(nameof(operationLog));
        }

        //TODO update details
        // operationLogToUpdate.UpdateDetails();
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<OperationLog>> GetAllAsync(int bankId)
    {
        return await _context.OperationLogs
            .Where(ol => ol.BankId == bankId)
            .ToListAsync();
    }
}