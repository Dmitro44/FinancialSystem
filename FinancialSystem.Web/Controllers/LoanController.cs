using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSystem.Web.Controllers;

[Authorize]
public class LoanController : BaseController
{
    [HttpPost("[controller]/CalculateTotalAmount")]
    public JsonResult CalculateTotalAmount([FromBody] BaseCalculationRequest request)
    {
        decimal interestRate = FinancialCalculator.CalculateInterestRate(request.TermInMonths);
        decimal totalAmount = FinancialCalculator.CalculateTotalAmount(request.Amount, request.TermInMonths);
        decimal monthlyPayment = FinancialCalculator.CalculateMonthlyPayment(totalAmount, request.TermInMonths);
        
        return Json(new { totalAmount, interestRate, monthlyPayment });
    } 
}