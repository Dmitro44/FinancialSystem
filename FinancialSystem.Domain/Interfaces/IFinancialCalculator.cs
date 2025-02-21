namespace FinancialSystem.Domain.Interfaces;

public interface IFinancialCalculator
{
    decimal CalculateTotalAmount(decimal amount, decimal interestRate);
    decimal CalculateMonthlyPayment(decimal totalAmount, decimal termInMonths);
}