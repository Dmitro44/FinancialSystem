using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Services;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;
using FinancialSystem.Web.Models;
using FinancialSystem.Web.Models.Client;
using FinancialSystem.Web.Models.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSystem.Web.Controllers;

[Authorize]
public class BankController : BaseController
{
    private readonly BankService _bankService;
    private readonly UserService _userService;

    public BankController(BankService bankService, UserService userService)
    {
        _bankService = bankService;
        _userService = userService;
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

    [HttpGet("Details/{bankId}")]
    public async Task<IActionResult> GetBankDetails(int bankId)
    {
        var currentUserId = GetCurrentUserId();
        
        var bank = await _bankService.GetBankByIdAsync(bankId);

        if (bank == null)
        {
            return NotFound("Bank not found");
        }

        var role = await _userService.GetRoleInBankAsync(currentUserId, bankId);
        
        if (role == Role.Client)
        {
            var userAccounts = await _bankService.RetrieveUserAccountsByBankAsync(currentUserId, bankId);
            var userLoans = await _bankService.RetrieveUserLoansByBankAsync(currentUserId, bankId);
            var userInstallments = await _bankService.RetrieveUserInstallmentsByBankAsync(currentUserId, bankId);

            ViewBag.BankId = bankId;
        
            var model = new ClientBankViewModel
            {
                BankId = bankId,
                BankName = bank.Name,
                Bic = bank.Bic,
                Address = bank.Address,
                Accounts = userAccounts.ToList(),
                Loans = userLoans.ToList(),
                Installments = userInstallments.ToList()
            };
            
            return View("Client/Index", model);
        }

        if (role == Role.Manager)
        {
            var loanRequests = await _bankService.RetrieveLoansByBankAsync(bankId);
            var installmentRequests = await _bankService.RetrieveInstallmentsByBankAsync(bankId);

            var model = new ManagerBankViewModel
            {
                BankId = bankId,
                BankName = bank.Name,
                LoanRequests = loanRequests.ToList(),
                InstallmentRequests = installmentRequests.ToList()
            };
            
            return View("Manager/Index", model);
        }

        return Forbid();
    }
}