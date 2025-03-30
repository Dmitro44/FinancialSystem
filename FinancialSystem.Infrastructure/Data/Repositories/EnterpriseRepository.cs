using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialSystem.Infrastructure.Data.Repositories;

public class EnterpriseRepository : IEnterpriseRepository
{
    private readonly ApplicationDbContext _context;

    public EnterpriseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Enterprise?> GetByIdAsync(int enterpriseId)
    {
        return await _context.Enterprises.FindAsync(enterpriseId);
    }

    public async Task AddAsync(Enterprise enterprise)
    {
        await _context.Enterprises.AddAsync(enterprise);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Enterprise enterprise)
    {
        var enterpriseToUpdate = await _context.Enterprises.FindAsync(enterprise.Id);
        if (enterpriseToUpdate == null)
        {
            throw new ArgumentNullException(nameof(enterpriseToUpdate));
        }
        
        enterpriseToUpdate.UpdateDetails(
            enterprise.Type,
            enterprise.Name,
            enterprise.Unp,
            enterprise.Address,
            enterprise.Bank);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int enterpriseId)
    {
        var enterpriseToDelete = await _context.Enterprises.FindAsync(enterpriseId);
        if (enterpriseToDelete == null)
        {
            throw new ArgumentNullException(nameof(enterpriseToDelete));
        }
        
        _context.Enterprises.Remove(enterpriseToDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<Enterprise?> GetUserEnterpriseByBankAsync(int currentUserId, int bankId)
    {
        var userEnterprise = await _context.UserEnterprises
            .Include(ue => ue.Enterprise)
            .Where(ue => ue.UserId == currentUserId)
            .FirstOrDefaultAsync(ue => ue.Enterprise.BankId == bankId);
        
        return userEnterprise?.Enterprise;
    }

    public async Task<IEnumerable<EnterpriseAccount>> GetEnterpriseAccountsByBankAsync(int enterpriseId, int bankId)
    {
        return await _context.EnterpriseAccounts
            .Where(ea => ea.BankId == bankId && ea.EnterpriseId == enterpriseId)
            .Include(ea => ea.Bank)
            .ToListAsync();
    }
}