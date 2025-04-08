using FinancialSystem.Application.Interfaces;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialSystem.Application.Services;

public class EnterpriseService : IEnterpriseService
{
    private readonly IEnterpriseRepository _enterpriseRepository;
    private readonly ILogger<EnterpriseService> _logger;

    public EnterpriseService(IEnterpriseRepository enterpriseRepository, ILogger<EnterpriseService> logger)
    {
        _enterpriseRepository = enterpriseRepository;
        _logger = logger;
    }

    public async Task<Enterprise?> FetchUserEnterpriseByBankAsync(int currentUserId, int bankId)
    {
        _logger.LogInformation("Fetching enterprise for user {UserId} in bank {BankId}",
            currentUserId, bankId);

        try
        {
            var enterprise = await _enterpriseRepository.GetUserEnterpriseByBankAsync(currentUserId, bankId);
            if (enterprise != null)
            {
                _logger.LogInformation("Successfully fetched enterprise for user {UserId} in bank {BankId}",
                    currentUserId, bankId);
            }
            else
            {
                _logger.LogInformation("User {UserId} has no enterprise in bank {BankId}",
                    currentUserId, bankId);
            }

            return enterprise;
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Failed to fetch enterprise for user {UserId} in bank {BankId}",
                currentUserId, bankId);
            throw;
        }
        
    }
}