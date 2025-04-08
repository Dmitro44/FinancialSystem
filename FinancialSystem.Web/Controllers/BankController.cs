using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Interfaces;
using FinancialSystem.Application.Services;
using FinancialSystem.Domain.Enums;
using FinancialSystem.Web.Models;
using FinancialSystem.Web.Models.Client;
using FinancialSystem.Web.Models.Manager;
using FinancialSystem.Web.Models.Operator;
using FinancialSystem.Web.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSystem.Web.Controllers;

[Authorize]
public class BankController : BaseController
{
    private readonly IBankService _bankService;
    private readonly IUserService _userService;
    private readonly IEnterpriseService _enterpriseService; 

    public BankController(IBankService bankService, IUserService userService, IEnterpriseService enterpriseService)
    {
        _bankService = bankService;
        _userService = userService;
        _enterpriseService = enterpriseService;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterToBank(int bankId, IFormCollection form)
    {
        var userId = GetCurrentUserId();
        var selectedRoleKey = $"SelectRole_{bankId}";

        if (!form.TryGetValue(selectedRoleKey, out var selectedRoleValue))
        {
            return BadRequest("Role not selected");
        }

        if (!Enum.TryParse<Role>(selectedRoleValue, out var role))
        {
            return BadRequest("Invalid role");
        }

        var userBankDto = new UserBankDto
        {
            UserId = userId,
            BankId = bankId,
            Role = role
        };
        
        await _bankService.RegisterUserToBankAsync(userBankDto);

        return RedirectToAction("GetBanks", "Bank");
    }

    [HttpGet]
    [Route("User/Banks")]
    public async Task<IActionResult> GetBanks()
    {
        var userId = GetCurrentUserId();
        var (registeredBanks, otherBanks) = await _bankService.GetUserBanksAsync(userId);

        var model = new UserBanksViewModel
        {
            RegisteredBanks = registeredBanks,
            OtherBanks = otherBanks
        };

        return View("~/Views/User/Banks.cshtml", model);
    }

    [HttpGet("Requests/{bankId}")]
    public async Task<IActionResult> PrepareLoanInstallmentRequests(int bankId)
    {
        var bank = await _bankService.GetBankByIdAsync(bankId);

        if (bank == null)
        {
            return NotFound("Bank not found");
        }

        var loanRequests = await _bankService.RetrieveLoansByBankAsync(bankId);
        var installmentRequests = await _bankService.RetrieveInstallmentsByBankAsync(bankId);

        var model = new ManagerRequestsViewModel
        {
            BankId = bankId,
            BankName = bank.Name,
            LoanRequests = loanRequests.ToList(),
            InstallmentRequests = installmentRequests.ToList()
        };

        return View("Manager/LoanInstallmentRequests/Index", model);
    }


    [HttpGet("Finances/{bankId}")]
    public async Task<IActionResult> PrepareClientFinances(int bankId)
    {
        var currentUserId = GetCurrentUserId();
        
        var bank = await _bankService.GetBankByIdAsync(bankId);

        if (bank == null)
        {
            return NotFound("Bank not found");
        }
        
        var userAccounts = await _bankService.RetrieveUserAccountsByBankAsync(currentUserId, bankId);
        var userLoans = await _bankService.RetrieveUserLoansByBankAsync(currentUserId, bankId);
        var userInstallments = await _bankService.RetrieveUserInstallmentsByBankAsync(currentUserId, bankId);
        
        ViewBag.BankId = bankId;

        var model = new ClientFinancesBankViewModel
        {
            BankId = bankId,
            BankName = bank.Name,
            Bic = bank.Bic,
            Address = bank.Address,
            Accounts = userAccounts.ToList(),
            Loans = userLoans.ToList(),
            Installments = userInstallments.ToList()
        };
        
        return View("Client/Finances/Index", model);
    }
    

    [HttpGet("RedirectToDashboard/{bankId}")]
    public async Task<IActionResult> RedirectToDashboard(int bankId)
    {
        var currentUserId = GetCurrentUserId();

        var role = await _userService.GetRoleInBankAsync(currentUserId, bankId);

        return role switch
        {
            Role.Client => RedirectToAction("ShowClientDashboard", "Client", new { bankId }),
            Role.Operator => RedirectToAction("ShowOperatorDashboard", "Operator", new { bankId }),
            Role.Manager => RedirectToAction("ShowManagerDashboard", "Manager", new { bankId }),
            Role.EnterpriseSpecialist => RedirectToAction("ShowEnterpriseSpecialistDashboard", "EnterpriseSpecialist", new { bankId }),
            Role.Administrator => RedirectToAction("ShowAdministratorDashboard", "Admin", new { bankId }),
            _ => Forbid()
        };
    }

    [HttpGet("TransferStatistics/{bankId}")]
    public async Task<IActionResult> TransferStatistics(int bankId)
    {
        var transfers = await _bankService.RetrieveTransfersByBankAsync(bankId);

        var model = new TransferStatisticsViewModel
        {
            TransfersFromBank = transfers.ToList(),
            ErrorMessage = TempData["ErrorMessage"]?.ToString(),
            SuccessMessage = TempData["SuccessMessage"]?.ToString()
        };
        
        var userRole = await _userService.GetRoleInBankAsync(GetCurrentUserId(), bankId);

        return userRole switch
        {
            Role.Operator => View("Operator/TransferStatistics/Index", model),
            Role.Manager => View("Manager/TransferStatistics/Index", model),
            _ => Forbid()
        };
    }

    [HttpGet("SalaryProjectRequests/{bankId}")]
    public async Task<IActionResult> PrepareSalaryProjectRequests(int bankId)
    {
        var bank = await _bankService.GetBankByIdAsync(bankId);
        if (bank == null)
        {
            return NotFound("Bank not found");
        }

        var salaryProjectRequests = await _bankService.RetrieveAllSalaryProjectsByBankAsync(bankId);

        ViewBag.BankId = bankId;
        
        var model = new OperatorRequestsViewModel
        {
            BankId = bankId,
            BankName = bank.Name,
            SalaryProjectRequests = salaryProjectRequests.ToList()
        };
        
        var userRole = await _userService.GetRoleInBankAsync(GetCurrentUserId(), bankId);

        return userRole switch
        {
            Role.Operator => View("Operator/SalaryProjectRequests/Index", model),
            Role.Manager => View("Manager/SalaryProjectRequests/Index", model),
            _ => Forbid()
        };
    }
}