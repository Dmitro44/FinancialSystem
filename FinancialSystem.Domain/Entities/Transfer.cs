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
    
    public Transfer() {}

    public Transfer(Account sender, Account receiver, decimal amount)
    {
        Sender = sender;
        SenderId = sender.Id;
        Receiver = receiver;
        ReceiverId = receiver.Id;
        Amount = amount;
        TransferDate = DateTime.Now;
    }
}