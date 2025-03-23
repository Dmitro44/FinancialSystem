using FinancialSystem.Application.Interfaces;
using FinancialSystem.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSystem.Web.Controllers.Manager;

[Authorize]
public class ManagerController : BaseController
{
    private readonly ILoanService _loanService;
    private readonly IInstallmentService _installmentService;

    public ManagerController(ILoanService loanService, IInstallmentService installmentService)
    {
        _loanService = loanService;
        _installmentService = installmentService;
    }

    [HttpGet("ManagerDashboard/{bankId}")]
    public async Task<ActionResult> ShowManagerDashboard(int bankId)
    {
        ViewBag.BankId = bankId;
        return View("~/Views/Bank/Manager/Dashboard.cshtml", new { bankId });
    }

    [HttpPost]
    public async Task<IActionResult> ApproveLoan(int loanId, int bankId)
    {
        await _loanService.UpdateStatusAsync(loanId, RequestStatus.Approved);
        return RedirectToAction("PrepareLoanInstallmentRequests", "Bank", new { bankId });
    }

    [HttpPost]
    public async Task<IActionResult> RejectLoan(int loanId, int bankId)
    {
        await _loanService.UpdateStatusAsync(loanId, RequestStatus.Rejected);
        return RedirectToAction("PrepareLoanInstallmentRequests", "Bank", new { bankId });
    }

    [HttpPost]
    public async Task<IActionResult> ApproveInstallment(int installmentId, int bankId)
    {
        await _installmentService.UpdateStatusAsync(installmentId, RequestStatus.Approved);
        return RedirectToAction("PrepareLoanInstallmentRequests", "Bank", new { bankId });
    }

    [HttpPost]
    public async Task<IActionResult> RejectInstallment(int installmentId, int bankId)
    {
        await _installmentService.UpdateStatusAsync(installmentId, RequestStatus.Rejected);
        return RedirectToAction("PrepareLoanInstallmentRequests", "Bank", new { bankId });
    }
}