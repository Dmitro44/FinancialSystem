using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialSystem.Infrastructure.Data.Repositories;

public class BankRepository : IBankRepository
{
    private readonly ApplicationDbContext _context;

    public BankRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Bank?> GetByIdAsync(int bankId)
    {
        return await _context.Banks.FindAsync(bankId);
    }

    public async Task AddAsync(Bank bank)
    {
        await _context.Banks.AddAsync(bank);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Bank bank)
    {
        var bankToUpdate = await _context.Banks.FindAsync(bank.Id);
        if (bankToUpdate == null)
        {
            throw new ArgumentNullException(nameof(bank));
        }

        bankToUpdate.UpdateDetails(
            bank.Name, 
            bank.Bic, 
            bank.Address);
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int bankId)
    {
        var bank = await _context.Banks.FindAsync(bankId);
        if (bank == null)
        {
            throw new ArgumentNullException(nameof(bank));
        }
        _context.Banks.Remove(bank);
        await _context.SaveChangesAsync();
    }

    public async Task AddUserToBankAsync(UserBankRole userBankRole)
    {
        await _context.UserBankRoles.AddAsync(userBankRole);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Bank>> GetAllBanksAsync()
    {
        return await _context.Banks.ToListAsync();
    }
}