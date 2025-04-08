namespace FinancialSystem.Application.DTOs;

public class SalaryPaymentResultDto
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public decimal TotalAmount { get; set; }
    public int EmployeesCount { get; set; }
    public int SuccessfulPayments { get; set; }
    public int FailedPayments { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
}