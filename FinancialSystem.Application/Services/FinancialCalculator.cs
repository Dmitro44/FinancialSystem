namespace FinancialSystem.Application.Services;

public static class FinancialCalculator
{
    public static decimal CalculateTotalAmount(decimal amount, int termInMonths)
    {
        var interestRate = CalculateInterestRate(termInMonths);
        
        return amount * (1 + interestRate / 100);
    }

    public static decimal CalculateMonthlyPayment(decimal totalAmount, int termInMonths)
    {
        return totalAmount / termInMonths;
    }

    public static decimal CalculateInterestRate(int termInMonths)
    {
        if (termInMonths <= 3)
            return 5.0m;
        if (termInMonths <= 6)
            return 6.0m;
        if (termInMonths <= 12)
            return 8.0m;
        if (termInMonths <= 24)
            return 10.0m;
        return 12.0m; // для сроков более 24 месяцев
    }
}