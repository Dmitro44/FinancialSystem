using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSystem.Web.Controllers;

public class InstallmentController : BaseController
{
    [HttpPost("[controller]/CalculateMonthlyPayment")]
    public JsonResult CalculateMonthlyPayment([FromBody] BaseCalculationRequest request)
    {
        var monthlyPayment = FinancialCalculator.CalculateMonthlyPayment(request.Amount, request.TermInMonths);
        return Json(new { monthlyPayment });
    }
}