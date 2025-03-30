using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Application.Interfaces;

public interface IEnterpriseService
{
    Task<Enterprise?> GetEnterpriseByIdAsync(int enterpriseId);
    Task<Enterprise?> FetchUserEnterpriseByBankAsync(int currentUserId, int bankId);
}