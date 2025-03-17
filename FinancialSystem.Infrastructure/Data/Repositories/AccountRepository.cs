using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialSystem.Infrastructure.Data.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _context;

    public AccountRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Account?> GetByIdAsync(int accountId)
    {
        return await _context.Accounts.FindAsync(accountId);
    }

    public async Task AddAsync(Account account)
    {
        await _context.Accounts.AddAsync(account);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Account account)
    {
        var accountToUpdate = await _context.Accounts.FindAsync(account.Id);
        if (accountToUpdate == null)
        {
            throw new ArgumentNullException(nameof(account));
        }
        
        accountToUpdate.UpdateDetails(account.Balance);
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int accountId)
    {
        var accountToDelete = await _context.Accounts.FindAsync(accountId);
        if (accountToDelete == null)
        {
            throw new ArgumentNullException(nameof(accountToDelete));
        }
        
        _context.Accounts.Remove(accountToDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Account>> GetUserAccountsByBankAsync(int userId, int bankId)
    {
        return await _context.Accounts
            .Include(a => a.Owner)
            .Where(account => account.OwnerId == userId && account.BankId == bankId)
            .ToListAsync();
    }
}