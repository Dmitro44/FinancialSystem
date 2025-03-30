using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Interfaces;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Interfaces;

namespace FinancialSystem.Application.Services;

public class EnterpriseAccountService : IEnterpriseAccountService
{
    private readonly IEnterpriseRepository _enterpriseRepository;
    private readonly IBankRepository _bankRepository;
    private readonly IEnterpriseAccountRepository _enterpriseAccountRepository;

    public EnterpriseAccountService(IEnterpriseRepository enterpriseRepository, IBankRepository bankRepository, IEnterpriseAccountRepository enterpriseAccountRepository)
    {
        _enterpriseRepository = enterpriseRepository;
        _bankRepository = bankRepository;
        _enterpriseAccountRepository = enterpriseAccountRepository;
    }

    public async Task CreateEnterpriseAccountAsync(EnterpriseAccountDto enterpriseAccountDto)
    {
        var enterprise = await _enterpriseRepository.GetByIdAsync(enterpriseAccountDto.EnterpriseId);
        if (enterprise == null)
        {
            throw new ApplicationException($"Enterprise with id: {enterpriseAccountDto.EnterpriseId} does not exist.");
        }
        
        var bank = await _bankRepository.GetByIdAsync(enterpriseAccountDto.BankId);
        if (bank == null)
        {
            throw new ApplicationException($"Bank with id: {enterpriseAccountDto.BankId} does not exist.");
        }

        var enterpriseAccount = new EnterpriseAccount(enterprise, enterpriseAccountDto.Balance, bank);

        await _enterpriseAccountRepository.AddAsync(enterpriseAccount);
    }

    public async Task<IEnumerable<EnterpriseAccount>> FetchEnterpriseAccountsByBankAsync(int enterpriseId, int bankId)
    {
        return await _enterpriseRepository.GetEnterpriseAccountsByBankAsync(enterpriseId, bankId);
    }
}