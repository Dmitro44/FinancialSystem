namespace FinancialSystem.Domain.Entities;

public class Installment
{
    public int Id { get; private set; }
    public User Payer { get; private set; }
    public Bank Bank { get; private set; }
    public decimal Amount { get; private set; }
    public int TermInMonths { get; private set; }
    public decimal InterestRate { get; private set; }
    public decimal MonthlyPayment { get; private set; }
    public DateTime StartDate { get; private set; }

    public Installment() {}
    public Installment(User payer, Bank bank, decimal amount,
        int termInMonths, decimal interestRate,
        decimal monthlyPayment, DateTime startDate)
    {
        Payer = payer;
        Bank = bank;
        Amount = amount;
        TermInMonths = termInMonths;
        InterestRate = interestRate;
        MonthlyPayment = monthlyPayment;
        StartDate = startDate;
    }
}