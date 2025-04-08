using Microsoft.AspNetCore.Mvc;

namespace FinancialSystem.Web.Controllers.Administrator;

public class AdminController : BaseController
{

    [HttpGet("AdminstratorDashboard/{bankId}")]
    public async Task<IActionResult> ShowAdministratorDashboard(int bankId)
    {
        ViewBag.BankId = bankId;
        return View("~/Views/Bank/Administrator/Dashboard.cshtml", new { bankId });
    }
    
    
}