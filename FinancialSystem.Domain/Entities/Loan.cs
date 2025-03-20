using FinancialSystem.Domain.Enums;

namespace FinancialSystem.Domain.Entities;

public class Loan
{
    public int Id { get; private set; }
    public User Borrower { get; private set; }
    public int BorrowerId { get; private set; }
    public Bank Bank { get; private set; }
    public int BankId { get; private set; }
    public decimal Amount { get; private set; }
    public int TermInMonths { get; private set; }
    public decimal InterestRate { get; private set; }
    public decimal TotalAmount { get; private set; }
    public decimal MonthlyPayment { get; private set; }
    public DateTime StartDate { get; private set; }
    public RequestStatus Status { get; private set; }

    public Loan() {}
    public Loan(User borrower, Bank bank, decimal amount,
        int termMonths, decimal interestRate, decimal totalAmount,
        decimal monthlyPayment)
    {
        Borrower = borrower;
        BorrowerId = borrower.Id;
        Bank = bank;
        BankId = bank.Id;
        Amount = amount;
        TermInMonths = termMonths;
        InterestRate = interestRate;
        TotalAmount = totalAmount;
        MonthlyPayment = monthlyPayment;
        StartDate = DateTime.UtcNow.AddHours(3);
        Status = RequestStatus.Pending;
    }

    public void UpdateDetails(decimal amount, int termInMonths,
        decimal interestRate, decimal totalAmount, 
        decimal monthlyPayment, DateTime startDate)
    {
        Amount = amount;
        TermInMonths = termInMonths;
        InterestRate = interestRate;
        TotalAmount = totalAmount;
        MonthlyPayment = monthlyPayment;
        StartDate = startDate;
    }

    public void SetStatus(RequestStatus status)
    {
        if (status == RequestStatus.Pending) return;
        
        Status = status;
    }
}