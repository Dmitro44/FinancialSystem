using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Services;
using FinancialSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSystem.Web.Controllers.Client;

public class ClientController : BaseController
{
    private readonly BankService _bankService;

    public ClientController(BankService bankService)
    {
        _bankService = bankService;
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
        
        return RedirectToAction("GetBankDetails", "Bank" , new { bankId = model.BankId });
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
        
        return RedirectToAction("GetBankDetails", "Bank", new { bankId = model.BankId });
    }
    
    public async Task<IActionResult> CreateInstallment()
    {
        throw new NotImplementedException();
    }
}