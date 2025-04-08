using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;
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

    public async Task<UserAccount?> GetUserAccountByIdAsync(int accountId)
    {
        return await _context.UserAccounts.FindAsync(accountId);
    }

    public async Task<EnterpriseAccount?> GetEnterpriseAccountByIdAsync(int accountId)
    {
        return await _context.EnterpriseAccounts.FindAsync(accountId);
    }

    public async Task<IEnumerable<UserAccount>> GetUserAccountsByBankAsync(int userId, int bankId)
    {
        return await _context.UserAccounts
            .Include(a => a.Owner)
            .Where(account => account.OwnerId == userId && account.BankId == bankId)
            .ToListAsync();
    }

    public async Task<UserAccount?> GetInactiveAccountBySalaryProjectAsync(int userId, int salaryProjectId)
    {
        var salaryProject = await _context.SalaryProjects.FirstOrDefaultAsync(sp => sp.Id == salaryProjectId);

        if (salaryProject == null)
        {
            return null;
        }
        
        return await _context.UserAccounts
            .Where(a => a.OwnerId == userId
                    && a.BankId == salaryProject.BankId
                    && a.AccountType == AccountType.Salary
                    && a.IsActive == false)
            .FirstOrDefaultAsync();
    }
}