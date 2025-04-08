using FinancialSystem.Domain.Enums;

namespace FinancialSystem.Domain.Entities;

public class Installment
{
    public int Id { get; private set; }
    public User Payer { get; private set; }
    public int PayerId { get; private set; }
    public Bank Bank { get; private set; }
    public int BankId { get; private set; }
    public decimal Amount { get; private set; }
    public int TermInMonths { get; private set; }
    public decimal MonthlyPayment { get; private set; }
    public DateTime StartDate { get; private set; }
    public RequestStatus Status { get; private set; }

    public UserAccount? InstallmentAccount { get; set; }
    public int? InstallmentAccountId { get; set; }

    public int DestinationAccountId { get; set; }

    public Installment() {}
    public Installment(User payer, Bank bank, decimal amount,
        int termInMonths, decimal monthlyPayment,
        DateTime startDate, int destinationAccountId)
    {
        Payer = payer;
        PayerId = payer.Id;
        Bank = bank;
        BankId = bank.Id;
        Amount = amount;
        TermInMonths = termInMonths;
        MonthlyPayment = monthlyPayment;
        StartDate = startDate;
        Status = RequestStatus.Pending;
        DestinationAccountId = destinationAccountId;
    }

    public void UpdateDetails(decimal amount, int termInMonths,
        decimal monthlyPayment, DateTime startDate)
    {
        Amount = amount;
        TermInMonths = termInMonths;
        MonthlyPayment = monthlyPayment;
        StartDate = startDate;
    }

    public void SetStatus(RequestStatus status)
    {
        if (status == RequestStatus.Pending) return;
        
        Status = status;
    }

    public void AddInstallmentAccount(UserAccount installmentAccount)
    {
        InstallmentAccountId = installmentAccount.Id;
        InstallmentAccount = installmentAccount;
    }
}