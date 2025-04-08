using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Application.Interfaces;

public interface IEnterpriseService
{
    Task<Enterprise?> FetchUserEnterpriseByBankAsync(int currentUserId, int bankId);
}