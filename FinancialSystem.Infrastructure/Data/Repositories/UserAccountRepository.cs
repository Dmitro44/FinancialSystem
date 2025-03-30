using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialSystem.Infrastructure.Data.Repositories;

public class UserAccountRepository : IUserAccountRepository
{
    private readonly ApplicationDbContext _context;

    public UserAccountRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserAccount?> GetByIdAsync(int accountId)
    {
        return await _context.UserAccounts.FindAsync(accountId);
    }

    public async Task AddAsync(UserAccount userAccount)
    {
        await _context.UserAccounts.AddAsync(userAccount);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserAccount userAccount)
    {
        var accountToUpdate = await _context.UserAccounts.FindAsync(userAccount.Id);
        if (accountToUpdate == null)
        {
            throw new ArgumentNullException(nameof(userAccount));
        }
        
        accountToUpdate.UpdateDetails(userAccount.Balance);
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int accountId)
    {
        var accountToDelete = await _context.UserAccounts.FindAsync(accountId);
        if (accountToDelete == null)
        {
            throw new ArgumentNullException(nameof(accountToDelete));
        }
        
        _context.UserAccounts.Remove(accountToDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserAccount>> GetUserAccountsByBankAsync(int userId, int bankId)
    {
        return await _context.UserAccounts
            .Include(a => a.Owner)
            .Where(account => account.OwnerId == userId && account.BankId == bankId)
            .ToListAsync();
    }
}