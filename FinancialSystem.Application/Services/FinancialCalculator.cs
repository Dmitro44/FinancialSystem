using FinancialSystem.Domain.Interfaces;

namespace FinancialSystem.Application.Services;

public class FinancialCalculator : IFinancialCalculator
{
    public decimal CalculateTotalAmount(decimal amount, decimal interestRate)
    {
        return amount * (1 + interestRate / 100);
    }

    public decimal CalculateMonthlyPayment(decimal totalAmount, decimal termInMonths)
    {
        return totalAmount / termInMonths;
    }
}