using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Interfaces;
using FinancialSystem.Domain.Enums;
using FinancialSystem.Web.Models.Client;
using FinancialSystem.Web.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSystem.Web.Controllers.Client;

[Authorize]
public class ClientController : BaseController
{
    private readonly IBankService _bankService;

    public ClientController(IBankService bankService)
    {
        _bankService = bankService;
    }

    [HttpGet("ClientDashboard/{bankId}")]
    public async Task<ActionResult> ShowClientDashboard(int bankId)
    {
        ViewBag.BankId = bankId;
        return View("~/Views/Bank/Client/Dashboard.cshtml", new { bankId });
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount(AccountViewModel model)
    {
        var currentUserId = GetCurrentUserId();
        
        var accountDto = new AccountDto
        {
            Balance = model.Balance,
            BankId = model.BankId,
            OwnerId = currentUserId
        };
        
        await _bankService.CreateAccountAsync(accountDto);
        
        return RedirectToAction("PrepareClientFinances", "Bank" , new { bankId = model.BankId });
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateLoan(LoanViewModel model)
    {
        var currentUserId = GetCurrentUserId();

        var loanDto = new LoanDto
        {
            Amount = model.Amount,
            BankId = model.BankId,
            InterestRate = model.InterestRate,
            TermInMonths = model.TermInMonths,
            TotalAmount = model.TotalAmount,
            MonthlyPayment = model.MonthlyPayment,
            UserId = currentUserId
        };
        
        await _bankService.CreateLoanAsync(loanDto);
        
        return RedirectToAction("PrepareClientFinances", "Bank", new { bankId = model.BankId });
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateInstallment(InstallmentViewModel model)
    {
        var currentUserId = GetCurrentUserId();

        var installmentDto = new InstallmentDto
        {
            Amount = model.Amount,
            BankId = model.BankId,
            TermInMonths = model.TermInMonths,
            UserId = currentUserId
        };
        
        await _bankService.CreateInstallmentAsync(installmentDto);
        
        return RedirectToAction("PrepareClientFinances", "Bank", new { bankId = model.BankId });
    }

    [HttpGet("Transfer/{bankId}")]
    public async Task<IActionResult> PrepareTransfer(int bankId)
    {
        var currentUserId = GetCurrentUserId();
        
        var userAccounts = await _bankService.RetrieveUserAccountsByBankAsync(currentUserId, bankId);

        var model = new TransferViewModel
        {
            BankId = bankId,
            FromAccounts = userAccounts.ToList(),
            ErrorMessage = TempData["ErrorMessage"]?.ToString()
        };
        
        ViewBag.BankId = bankId;
        
        return View("~/Views/Bank/Client/Transfer/Index.cshtml", model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransfer(TransferViewModel model)
    {
        var transferDto = new TransferDto
        {
            Amount = model.Amount,
            ReceiverId = model.ReceiverId,
            SenderId = model.SenderId,
            Status = TransferStatus.Active
        };

        try
        {
            await _bankService.CreateTransferAsync(transferDto);
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;

            return RedirectToAction("PrepareTransfer", "Client", new { bankId = model.BankId });
        }
        
        return RedirectToAction("PrepareTransfer", "Client", new { bankId = model.BankId });
    }
}