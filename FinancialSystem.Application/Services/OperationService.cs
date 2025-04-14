using FinancialSystem.Application.Interfaces;
using FinancialSystem.Domain.Interfaces;
using FinancialSystem.Domain.Operations;
using Microsoft.Extensions.Logging;

namespace FinancialSystem.Application.Services;

public class OperationService
{
    private readonly IOperationLogRepository _logRepository;
    private readonly ILoanService _loanService;
    private readonly IInstallmentService _installmentService;
    private readonly ISalaryProjectService _salaryProjectService;
    private readonly IUserService _userService;
    private readonly ILogger<OperationService> _logger;

    public OperationService(
        IOperationLogRepository logRepository,
        ILoanService loanService,
        IInstallmentService installmentService,
        ISalaryProjectService salaryProjectService,
        IUserService userService,
        ILogger<OperationService> logger)
    {
        _logRepository = logRepository;
        _loanService = loanService;
        _installmentService = installmentService;
        _salaryProjectService = salaryProjectService;
        _userService = userService;
        _logger = logger;
    }

    public async Task LogOperationAsync(string operationType, int entityId, int userId, int bankId, string? comment = null)
    {
        var log = new OperationLog(operationType, entityId, userId, bankId, comment);
        await _logRepository.AddAsync(log);
        
        _logger.LogInformation("Logged operation {OperationType} on entity {EntityId} by user {UserId}",
            operationType, entityId, userId);
    }

    public async Task<bool> RevertOperationAsync(int logId, int currentUserId, string comment)
    {
        var log = await _logRepository.GetByIdAsync(logId);
        if (log == null || log.Status != OperationStatus.Active)
        {
            _logger.LogWarning("Cannot revert operation: log with ID {LogId} not found or not active", logId);
            return false;
        }

        try
        {
            bool success;
            switch (log.OperationType)
            {
                case "LoanCreation":
                    success = await _loanService.RevertLoanCreationAsync(log.EntityId);
                    break;
                case "InstallmentCreation":
                    success = await _installmentService.RevertInstallmentCreationAsync(log.EntityId);
                    break;
                case "SalaryProjectCreation":
                    success = await _salaryProjectService.RevertSalaryProjectCreationAsync(log.EntityId);
                    break;
                default:
                    _logger.LogWarning("Unsupported operation type for reversal: {OperationType}", log.OperationType);
                    return false;
            }

            if (success)
            {
                var reversalLog = OperationLog.CreateReversalLog(log, currentUserId, comment);
                await _logRepository.AddAsync(reversalLog);
            
                log.MarkAsReverted(reversalLog.Id);
                await _logRepository.UpdateAsync(log);
            
                _logger.LogInformation(
                    "Successfully reverted operation {LogId} of type {OperationType} on entity {EntityId}",
                    log.Id, log.OperationType, log.EntityId);
            }
        
            return success;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reverting operation {LogId} of type {OperationType} on entity {EntityId}",
                log.Id, log.OperationType, log.EntityId);
            return false;
        }
    }

    public async Task<bool> RestoreOperationAsync(int logId, int currentUserId, string comment)
    {
        var log = await _logRepository.GetByIdAsync(logId);
        if (log == null || log.Status == OperationStatus.Active)
        {
            _logger.LogWarning(
                "Cannot restore operation: log with ID {LogId} not found, not reverted, or has no reversal log",
                logId);
            return false;
        }

        var reversalLog = await _logRepository.GetByIdAsync(log.ReversalLogId.Value);
        if (reversalLog == null)
        {
            _logger.LogWarning("Cannot restore operation: reversal log with ID {ReversalLogId} not found", 
                log.ReversalLogId.Value);
            return false;
        }

        try
        {
            bool success;
            switch (log.OperationType)
            {
                case "LoanCreation":
                    success = await _loanService.RestoreLoanCreationAsync(log.EntityId);
                    break;
                case "InstallmentCreation":
                    success = await _installmentService.RestoreInstallmentCreationAsync(log.EntityId);
                    break;
                case "SalaryProjectCreation":
                    success = await _salaryProjectService.RestoreSalaryProjectCreationAsync(log.EntityId);
                    break;
                default:
                    _logger.LogWarning("Unsupported operation type for restoration: {OperationType}", log.OperationType);
                    return false;
            }

            if (success)
            {
                var restorationLog = OperationLog.CreateRestorationLog(reversalLog, currentUserId, comment);
                await _logRepository.AddAsync(restorationLog);
            
                log.MarkAsRestored();
                await _logRepository.UpdateAsync(log);
            
                _logger.LogInformation(
                    "Successfully restored operation {LogId} of type {OperationType} on entity {EntityId}",
                    log.Id, log.OperationType, log.EntityId);
            }   
        
            return success;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error restoring operation {LogId} of type {OperationType} on entity {EntityId}",
                log.Id, log.OperationType, log.EntityId);
            return false;
        }
    }

    public async Task<IEnumerable<OperationLog>> GetAllAsync(int bankId)
    {
        return await _logRepository.GetAllAsync(bankId);
    }
}