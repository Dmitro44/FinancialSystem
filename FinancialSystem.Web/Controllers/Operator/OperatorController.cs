using FinancialSystem.Application.Interfaces;
using FinancialSystem.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSystem.Web.Controllers.Operator;

[Authorize]
[Route("Operator/[action]")]
public class OperatorController : BankStaffBaseController
{
    public OperatorController(ITransferService transferService, ISalaryProjectService salaryProjectService)
        :base(transferService, salaryProjectService)
    {
    }

    [HttpGet("OperatorDashboard/{bankId}")]
    public async Task<IActionResult> ShowOperatorDashboard(int bankId)
    {
        ViewBag.BankId = bankId;
        return View("~/Views/Bank/Operator/Dashboard.cshtml", new { bankId });
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