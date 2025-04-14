using FinancialSystem.Application.Services;
using FinancialSystem.Web.Models.Administrator;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSystem.Web.Controllers.Administrator;

public class AdminController : BaseController
{
    private readonly OperationService _operationService;

    public AdminController(OperationService operationService)
    {
        _operationService = operationService;
    }

    [HttpGet("AdministratorDashboard/{bankId}")]
    public IActionResult ShowAdministratorDashboard(int bankId)
    {
        ViewBag.BankId = bankId;
        return View("~/Views/Bank/Administrator/Dashboard.cshtml", new { bankId });
    }

    [HttpGet]
    public async Task<IActionResult> ShowOperations(int bankId)
    {
        var operations = (await _operationService.GetAllAsync(bankId)).ToList();

        var model = new OperationViewModel
        {
            Operations = operations
        };
        
        return View("~/Views/Bank/Administrator/Operations/Index.cshtml", model);
    }

    [HttpPost]
    public async Task<IActionResult> RevertOperation(OperationViewModel model)
    {
        var currentUserId = GetCurrentUserId();
        
        var result = await _operationService.RevertOperationAsync(model.LogId, currentUserId, model.Comment);
        
        TempData["OperationSuccess"] = result;
        TempData["OperationMessage"] = result
            ? "Operation successfully reverted."
            : "Operation failed to revert.";
        
        return RedirectToAction("ShowOperations", "Admin", new { model.BankId });
    }

    [HttpPost]
    public async Task<IActionResult> RestoreOperation(OperationViewModel model)
    {
        var currentUserId = GetCurrentUserId();
        
        var result = await _operationService.RestoreOperationAsync(model.LogId, currentUserId, model.Comment);
        
        TempData["OperationSuccess"] = result;
        TempData["OperationMessage"] = result
            ? "Operation successfully restored."
            : "Operation failed to restore.";
        
        return RedirectToAction("ShowOperations", "Admin", new { model.BankId });
    }
}