using FinancialSystem.Application.Interfaces;
using FinancialSystem.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSystem.Web.Controllers;

public class BankStaffBaseController : BaseController
{
    protected readonly ITransferService _transferService;
    protected readonly ISalaryProjectService _salaryProjectService;

    protected BankStaffBaseController(ITransferService transferService, ISalaryProjectService salaryProjectService)
    {
        _transferService = transferService;
        _salaryProjectService = salaryProjectService;
    }
    
    public async Task<IActionResult> CancelTransferBase(int transferId)
    {
        var result = await _transferService.CancelTransferAsync(transferId);

        if (result.success)
        {
            TempData["SuccessMessage"] = result.message;
        }
        else
        {
            TempData["ErrorMessage"] = result.message;
            if (result.message == "Transfer not found")
            {
                return NotFound(result.message);
            }
        }

        return RedirectToAction("TransferStatistics", "Bank", new { result.bankId });
    }
    
    [HttpPost]
    public async Task<IActionResult> ApproveSalaryProjectBase(int projectId, int bankId)
    {
        await _salaryProjectService.UpdateStatusAsync(projectId, SalaryProjectStatus.Approved);
        return RedirectToAction("PrepareSalaryProjectRequests", "Bank", new { bankId });
    }
    
    [HttpPost]
    public async Task<IActionResult> RejectSalaryProjectBase(int projectId, int bankId)
    {
        await _salaryProjectService.UpdateStatusAsync(projectId, SalaryProjectStatus.Rejected);
        return RedirectToAction("PrepareSalaryProjectRequests", "Bank", new { bankId });
    }
}