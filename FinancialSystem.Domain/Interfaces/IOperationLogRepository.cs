using FinancialSystem.Domain.Operations;

namespace FinancialSystem.Domain.Interfaces;

public interface IOperationLogRepository
{
    Task<OperationLog?> GetByIdAsync(int logId);
    Task AddAsync(OperationLog operationLog);
    Task UpdateAsync(OperationLog operationLog);
    Task<IEnumerable<OperationLog>> GetAllAsync(int bankId);
}