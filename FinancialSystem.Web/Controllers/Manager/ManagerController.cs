using FinancialSystem.Application.Services;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSystem.Web.Controllers.Manager;

public class ManagerController : BaseController
{
    private readonly LoanService _loanService;
    private readonly InstallmentService _installmentService;

    public ManagerController(LoanService loanService, InstallmentService installmentService)
    {
        _loanService = loanService;
        _installmentService = installmentService;
    }

    [HttpPost]
    public async Task<IActionResult> ApproveLoan(int loanId, int bankId)
    {
        await _loanService.UpdateStatusAsync(loanId, RequestStatus.Approved);
        return RedirectToAction("GetBankDetails", "Bank", new { bankId });
    }

    [HttpPost]
    public async Task<IActionResult> RejectLoan(int loanId, int bankId)
    {
        await _loanService.UpdateStatusAsync(loanId, RequestStatus.Rejected);
        return RedirectToAction("GetBankDetails", "Bank", new { bankId });
    }

    [HttpPost]
    public async Task<IActionResult> ApproveInstallment(int installmentId, int bankId)
    {
        await _installmentService.UpdateStatusAsync(installmentId, RequestStatus.Approved);
        return RedirectToAction("GetBankDetails", "Bank", new { bankId });
    }

    [HttpPost]
    public async Task<IActionResult> RejectInstallment(int installmentId, int bankId)
    {
        await _installmentService.UpdateStatusAsync(installmentId, RequestStatus.Rejected);
        return RedirectToAction("GetBankDetails", "Bank", new { bankId });
    }
}