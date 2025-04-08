using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Interfaces;
using FinancialSystem.Web.Models.EnterpriseSpecialist;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSystem.Web.Controllers.EnterpriseSpecialist;

public class EnterpriseSpecialistController : BaseController
{
    private readonly IEnterpriseService _enterpriseService;
    private readonly IEnterpriseAccountService _enterpriseAccountService;
    private readonly ISalaryProjectService _salaryProjectService;

    public EnterpriseSpecialistController(IEnterpriseService enterpriseService, IEnterpriseAccountService enterpriseAccountService, ISalaryProjectService salaryProjectService)
    {
        _enterpriseService = enterpriseService;
        _enterpriseAccountService = enterpriseAccountService;
        _salaryProjectService = salaryProjectService;
    }

    [HttpGet("EnterpriseSpecialistDashboard/{bankId}")]
    public IActionResult ShowEnterpriseSpecialistDashboard(int bankId)
    {
        ViewBag.BankId = bankId;
        return View("~/Views/Bank/EnterpriseSpecialist/Dashboard.cshtml", new { bankId });
    }

    [HttpGet("EnterpriseAccounts/{bankId}")]
    public async Task<IActionResult> EnterpriseAccounts(int bankId)
    {
        var currentUserId = GetCurrentUserId();
        var enterprise = await _enterpriseService.FetchUserEnterpriseByBankAsync(currentUserId, bankId);
        if (enterprise == null)
        {
            return BadRequest("Enterprise not found for the current user and specified bank.");
        }
        
        var enterpriseAccounts = await _enterpriseAccountService.FetchEnterpriseAccountsByBankAsync(enterprise.Id, bankId);

        ViewBag.BankId = bankId;
        
        var model = new EnterpriseFinancesViewModel
        {
            BankId = bankId,
            EnterpriseAddress = enterprise.Address,
            EnterpriseName = enterprise.Name,
            EnterpriseType = enterprise.Type,
            EnterpriseUnp = enterprise.Unp,
            EnterpriseAccounts = enterpriseAccounts.ToList()
        };
        
        return View("~/Views/Bank/EnterpriseSpecialist/EnterpriseAccount/Index.cshtml", model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEnterpriseAccount(EnterpriseAccountViewModel model)
    {
        var currentUserId = GetCurrentUserId();
        var enterprise = await _enterpriseService.FetchUserEnterpriseByBankAsync(currentUserId, model.BankId);
        if (enterprise == null)
        {
            return BadRequest("Enterprise not found for the current user and specified bank.");
        }

        var enterpriseAccountDto = new EnterpriseAccountDto
        {
            EnterpriseId = enterprise.Id,
            BankId = model.BankId,
            Balance = model.Balance
        };

        await _enterpriseAccountService.CreateEnterpriseAccountAsync(enterpriseAccountDto);
        
        return RedirectToAction("EnterpriseAccounts", "EnterpriseSpecialist", new { bankId = model.BankId });
    }

    [HttpGet("SalaryProjects/{bankId}")]
    public async Task<IActionResult> SalaryProjects(int bankId)
    {
        var currentUserId = GetCurrentUserId();
        var enterprise = await _enterpriseService.FetchUserEnterpriseByBankAsync(currentUserId, bankId);
        if (enterprise == null)
        {
            return BadRequest("Enterprise not found for the current user and specified bank.");
        }

        var salaryProjects = await _salaryProjectService.FetchApprovedSalaryProjectsByBankAsync(enterprise.Id, bankId);
        var enterpriseAccounts = await _enterpriseAccountService.FetchEnterpriseAccountsByBankAsync(enterprise.Id, bankId);
        
        ViewBag.BankId = bankId;
        
        var model = new EnterpriseFinancesViewModel
        {
            BankId = bankId,
            EnterpriseAddress = enterprise.Address,
            EnterpriseName = enterprise.Name,
            EnterpriseType = enterprise.Type,
            EnterpriseUnp = enterprise.Unp,
            EnterpriseAccounts = enterpriseAccounts.ToList(),
            SalaryProjects = salaryProjects.ToList()
        };
        
        return View("~/Views/Bank/EnterpriseSpecialist/SalaryProject/Index.cshtml", model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSalaryProject(SalaryProjectViewModel model)
    {
        var currentUserId = GetCurrentUserId();
        var enterprise = await _enterpriseService.FetchUserEnterpriseByBankAsync(currentUserId, model.BankId);
        if (enterprise == null)
        {
            return BadRequest("Enterprise not found for the current user and specified bank.");
        }

        var salaryProjectDto = new SalaryProjectDto
        {
            EnterpriseId = enterprise.Id,
            BankId = model.BankId,
            EnterpriseAccountId = model.EnterpriseAccountId,
            Salary = model.Salary
        };

        await _salaryProjectService.CreateSalaryProjectAsync(salaryProjectDto);

        return RedirectToAction("SalaryProjects", "EnterpriseSpecialist", new { bankId = model.BankId });
    }

    [HttpPost("ProcessProjects/{bankId}/{salaryProjectId}")]
    public async Task<IActionResult> ProcessSalaryPayments(int bankId, int salaryProjectId)
    {
        var result = await _salaryProjectService.ProcessSalaryPaymentsAsync(salaryProjectId);
        
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Заработная плата успешно выплачена";
        }
        else
        {
            TempData["ErrorMessage"] = "Произошли ошибки при выплате заработной платы";
        }
    
        TempData["PaymentStats"] = $"Сумма: {result.TotalAmount}, Сотрудников: {result.EmployeesCount}, Успешно: {result.SuccessfulPayments}, Ошибок: {result.FailedPayments}";
    
        if (result.Errors.Any())
        {
            TempData["PaymentErrors"] = string.Join("<br>", result.Errors);
        }
        
        return RedirectToAction("SalaryProjects", "EnterpriseSpecialist", new { bankId });
    }
}