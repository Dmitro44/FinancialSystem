using FinancialSystem.Domain.Enums;

namespace FinancialSystem.Domain.Entities;

public class Transfer
{
    public int Id { get; private set; }
    public Account Sender { get; private set; }
    public int SenderId { get; private set; }
    public Account Receiver { get; private set; }
    public int ReceiverId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime TransferDate { get; private set; }
    public TransferStatus Status { get; private set; }
    public TransferType Type { get; set; }
    
    public Transfer() {}

    public Transfer(Account sender, Account receiver, decimal amount, TransferStatus status, TransferType type)
    {
        Sender = sender;
        SenderId = sender.Id;
        Receiver = receiver;
        ReceiverId = receiver.Id;
        Amount = amount;
        TransferDate = DateTime.UtcNow.AddHours(3);
        Status = status;
        Type = type;
    }

    public void SetStatus(TransferStatus status)
    {
        if (Status == TransferStatus.Canceled) return;
        
        Status = status;
    }
}