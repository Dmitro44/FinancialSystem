using FinancialSystem.Application.Interfaces;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Interfaces;

namespace FinancialSystem.Application.Services;

public class EnterpriseService : IEnterpriseService
{
    private readonly IEnterpriseRepository _enterpriseRepository;

    public EnterpriseService(IEnterpriseRepository enterpriseRepository)
    {
        _enterpriseRepository = enterpriseRepository;
    }

    public async Task<Enterprise?> GetEnterpriseByIdAsync(int enterpriseId)
    {
        return await _enterpriseRepository.GetByIdAsync(enterpriseId);
    }

    public async Task<Enterprise?> FetchUserEnterpriseByBankAsync(int currentUserId, int bankId)
    {
        return await _enterpriseRepository.GetUserEnterpriseByBankAsync(currentUserId, bankId);
    }
}