using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Services;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSystem.Web.Controllers;

[Authorize]
public class BankController : BaseController
{
    private readonly BankService _bankService;

    public BankController(BankService bankService)
    {
        _bankService = bankService;
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
        
        var userAccounts = await _bankService.GetAccountsForUserAsync(currentUserId);
        var userLoans = await _bankService.GetLoansForUserAsync(currentUserId);
        var userInstallments = await _bankService.GetInstallmentsForUserAsync(currentUserId);
        
        var model = new BankViewModel
        {
            BankName = bank.Name,
            Bic = bank.Bic,
            Adress = bank.Address,
            Accounts = userAccounts.ToList(),
            Loans = userLoans.ToList(),
            Installments = userInstallments.ToList()
        };
            
        return View("~/Views/Bank/Index.cshtml", model);
    }

    public async Task<IActionResult> CreateAccount(AccountViewModel model)
    {
        throw new NotImplementedException();
    }

    public async Task<IActionResult> CreateLoan()
    {
        throw new NotImplementedException();
    }

    public async Task<IActionResult> CreateInstallment()
    {
        throw new NotImplementedException();
    }
}