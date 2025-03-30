using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Interfaces;

namespace FinancialSystem.Infrastructure.Data.Repositories;

public class EnterpriseAccountRepository : IEnterpriseAccountRepository
{
    private readonly ApplicationDbContext _context;

    public EnterpriseAccountRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EnterpriseAccount?> GetByIdAsync(int enterpriseAccountId)
    {
        return await _context.EnterpriseAccounts.FindAsync(enterpriseAccountId);
    }

    public async Task AddAsync(EnterpriseAccount entepriseAccount)
    {
        await _context.EnterpriseAccounts.AddAsync(entepriseAccount);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(EnterpriseAccount enterpriseAccount)
    {
        var enterpriseAccountToUpdate = await _context.EnterpriseAccounts.FindAsync(enterpriseAccount.Id);
        if (enterpriseAccountToUpdate == null)
        {
            throw new ArgumentNullException(nameof(enterpriseAccountToUpdate));
        }
        
        enterpriseAccountToUpdate.UpdateDetails(enterpriseAccount.Balance);
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int entepriseAccountId)
    {
        var enterpriseAccountToDelete = await _context.EnterpriseAccounts.FindAsync(entepriseAccountId);
        if (enterpriseAccountToDelete == null)
        {
            throw new ArgumentNullException(nameof(enterpriseAccountToDelete));
        }
        
        _context.EnterpriseAccounts.Remove(enterpriseAccountToDelete);
        await _context.SaveChangesAsync();
    }
}