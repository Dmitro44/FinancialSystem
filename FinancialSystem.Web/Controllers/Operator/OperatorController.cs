using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Interfaces;
using FinancialSystem.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSystem.Web.Controllers.Operator;

[Authorize]
public class OperatorController : BaseController
{
    private readonly IBankService _bankService;
    private readonly ITransferService _transferService;

    public OperatorController(ITransferService transferService, IBankService bankService)
    {
        _transferService = transferService;
        _bankService = bankService;
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
        var transfer = await _transferService.GetByIdAsync(transferId);
        if (transfer == null) return NotFound("Transfer not found");
    
        var transferDto = new TransferDto
        {
            SenderId = transfer.ReceiverId,
            ReceiverId = transfer.SenderId,
            Amount = transfer.Amount,
            Status = TransferStatus.Revert
        };

        try
        {
            await _bankService.CreateTransferAsync(transferDto);
            await _transferService.UpdateStatusAsync(transferId, TransferStatus.Canceled);
            TempData["SuccessMessage"] = "Transfer has been canceled";
            return RedirectToAction("TransferStatistics", "Bank", new { bankId = transfer.Sender.BankId });
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
            return RedirectToAction("TransferStatistics", "Bank", new { bankId = transfer.Sender.BankId });
        }
    }
}