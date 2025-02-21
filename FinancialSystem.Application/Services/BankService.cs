using FinancialSystem.Application.DTOs;
using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Application.Services;

public class BankService
{
    public Bank CreateBank(BankDto dto)
    {
        return new Bank(dto.Name, dto.Bic, dto.Address);
    }
}