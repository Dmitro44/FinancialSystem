using FinancialSystem.Application.Interfaces;
using FinancialSystem.Application.Services;
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
    private readonly OperationService _operationService;

    public ManagerController(ILoanService loanService, IInstallmentService installmentService, ISalaryProjectService salaryProjectService, ITransferService transferService, OperationService operationService)
        :base(transferService, salaryProjectService)
    {
        _loanService = loanService;
        _installmentService = installmentService;
        _operationService = operationService;
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

        await _operationService.LogOperationAsync(
            "LoanCreation",
            loanId,
            GetCurrentUserId(),
            bankId,
            $"Creation of loan {loanId}");
        
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
        
        await _operationService.LogOperationAsync(
            "InstallmentCreation",
            installmentId,
            GetCurrentUserId(),
            bankId,
            $"Creation of installment {installmentId}");
        
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
        await _operationService.LogOperationAsync(
            "SalaryProjectCreation",
            projectId,
            GetCurrentUserId(),
            bankId,
            $"Creation of salary project {projectId}");
        
        return await ApproveSalaryProjectBase(projectId, bankId);
    }
    
    [HttpPost]
    public async Task<IActionResult> RejectSalaryProject(int projectId, int bankId)
    {
        return await RejectSalaryProjectBase(projectId, bankId);
    }
}