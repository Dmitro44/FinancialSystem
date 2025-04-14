namespace FinancialSystem.Domain.Operations;

public class OperationLog
{
    public int Id { get; private set; }
    public string OperationType { get; private set; }
    public int EntityId { get; private set; }
    public int UserId { get; private set; }
    public int BankId { get; private set; }
    public DateTime TimeStamp { get; private set; }
    public OperationStatus Status { get; private set; }
    public string? Comment { get; private set; }
    public int? ReversalLogId { get; private set; }
    public int? OriginalLogId { get; private set; }
    
    private OperationLog() {}

    public OperationLog(string operationType, int entityId, int userId, int bankId, string? comment = null)
    {
        OperationType = operationType;
        EntityId = entityId;
        UserId = userId;
        BankId = bankId;
        Comment = comment;
        TimeStamp = DateTime.UtcNow.AddHours(3);
        Status = OperationStatus.Active;
    }

    public void UpdateDetails()
    {
        throw new NotImplementedException();
    }

    public void MarkAsReverted(int reversalLogId)
    {
        Status = OperationStatus.Reverted;
        ReversalLogId = reversalLogId;
    }

    public void MarkAsRestored()
    {
        Status = OperationStatus.Restored;
    }

    public static OperationLog CreateReversalLog(OperationLog originalLog, int userId, string comment)
    {
        var reversalLog = new OperationLog(
            $"Reversal_{originalLog.OperationType}",
            originalLog.EntityId,
            userId,
            originalLog.BankId,
            comment);
        
        reversalLog.OriginalLogId = originalLog.Id;
        reversalLog.Status = OperationStatus.ReversalOperation;

        return reversalLog;
    }

    public static OperationLog CreateRestorationLog(OperationLog originalLog, int userId, string comment)
    {
        var restorationLog = new OperationLog(
            $"Restoration_{originalLog.OperationType}",
            originalLog.EntityId,
            userId,
            originalLog.BankId,
            comment);
        
        restorationLog.OriginalLogId = originalLog.Id;
        restorationLog.Status = OperationStatus.RestorationOperation;
        
        return restorationLog;
    }
}

public enum OperationStatus
{
    Active,
    Reverted,
    Restored,
    ReversalOperation,
    RestorationOperation
}