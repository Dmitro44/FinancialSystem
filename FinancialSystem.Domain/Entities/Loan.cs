namespace FinancialSystem.Domain.Entities;

public class Loan
{
    public int Id { get; private set; }
    public User Borrower { get; private set; }
    public Bank Bank { get; private set; }
    public decimal Amount { get; private set; }
    public int TermInMonths { get; private set; }
    public decimal InterestRate { get; private set; }
    public decimal TotalAmount { get; private set; }
    public decimal MonthlyPayment { get; private set; }
    public DateTime StartDate { get; private set; }

    public Loan() {}
    public Loan(User borrower, Bank bank, decimal amount,
        int termMonths, decimal interestRate, decimal totalAmount,
        decimal monthlyPayment, DateTime startDate)
    {
        Borrower = borrower;
        Bank = bank;
        Amount = amount;
        TermInMonths = termMonths;
        InterestRate = interestRate;
        TotalAmount = totalAmount;
        MonthlyPayment = monthlyPayment;
        StartDate = startDate;
    }
}