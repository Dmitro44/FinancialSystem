using FinancialSystem.Application.DTOs;
using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Application.Interfaces;

public interface IEnterpriseAccountService
{
    Task CreateEnterpriseAccountAsync(EnterpriseAccountDto enterpriseAccountDto);
    Task<IEnumerable<EnterpriseAccount>> FetchEnterpriseAccountsByBankAsync(int enterpriseId, int bankId);
}