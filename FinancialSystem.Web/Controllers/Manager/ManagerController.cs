using FinancialSystem.Application.Services;
using FinancialSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSystem.Web.Controllers.Manager;

public class ManagerController : BaseController
{
    private readonly LoanService _loanService;

    public ManagerController(LoanService loanService)
    {
        _loanService = loanService;
    }

    [HttpPost]
    public async Task<IActionResult> ApproveLoan(int loanId, int bankId)
    {
        await _loanService.UpdateStatusAsync(loanId, LoanStatus.Approved);
        return RedirectToAction("GetBankDetails", "Bank", new { bankId });
    }

    [HttpPost]
    public async Task<IActionResult> RejectLoan(int loanId, int bankId)
    {
        await _loanService.UpdateStatusAsync(loanId, LoanStatus.Rejected);
        return RedirectToAction("GetBankDetails", "Bank", new { bankId });
    }
}