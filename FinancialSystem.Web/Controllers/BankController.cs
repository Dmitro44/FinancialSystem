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

        return RedirectToAction("Banks", "Bank");
    }

    [HttpGet]
    [ActionName("Banks")]
    public async Task<IActionResult> GetBanks()
    {
        var userId = GetCurrentUserId();
        var (registeredBanks, otherBanks) = await _bankService.GetUserBanksAsync(userId);

        var model = new BanksViewModel
        {
            RegisteredBanks = registeredBanks,
            OtherBanks = otherBanks
        };

        return View("~/Views/User/Banks.cshtml",model);
    }
}