using FinancialSystem.Application.Interfaces;
using FinancialSystem.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSystem.Web.Controllers.Manager;

[Authorize]
[Route("Manager/[action]")]
public class ManagerController : BankStaffBaseController
{
    private readonly ILoanService _loanService;
    private readonly IInstallmentService _installmentService;

    public ManagerController(ILoanService loanService, IInstallmentService installmentService, ISalaryProjectService salaryProjectService, ITransferService transferService)
        :base(transferService, salaryProjectService)
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
        await _loanService.AddLoanAccount(loanId);
        await _loanService.SendLoanAmount(loanId);
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
        await _installmentService.AddInstallmentAccount(installmentId);
        await _installmentService.SendInstallmentAmount(installmentId);
        return RedirectToAction("PrepareLoanInstallmentRequests", "Bank", new { bankId });
    }

    [HttpPost]
    public async Task<IActionResult> RejectInstallment(int installmentId, int bankId)
    {
        await _installmentService.UpdateStatusAsync(installmentId, RequestStatus.Rejected);
        return RedirectToAction("PrepareLoanInstallmentRequests", "Bank", new { bankId });
    }
    
    [HttpPost("CancelTransfer/{transferId}")]
    public async Task<IActionResult> CancelTransfer(int transferId)
    {
        return await CancelTransferBase(transferId);
    }

    [HttpPost]
    public async Task<IActionResult> ApproveSalaryProject(int projectId, int bankId)
    {
        return await ApproveSalaryProjectBase(projectId, bankId);
    }
    
    [HttpPost]
    public async Task<IActionResult> RejectSalaryProject(int projectId, int bankId)
    {
        return await RejectSalaryProjectBase(projectId, bankId);
    }
}