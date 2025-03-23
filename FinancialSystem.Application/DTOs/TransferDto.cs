using FinancialSystem.Domain.Enums;

namespace FinancialSystem.Application.DTOs;

public class TransferDto
{
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public decimal Amount { get; set; }
    public TransferStatus Status { get; set; }
}