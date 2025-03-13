using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialSystem.Infrastructure.Data.Repositories;

public class InstallmentRepository : IInstallmentRepository
{
    private readonly ApplicationDbContext _context;

    public InstallmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Installment?> GetByIdAsync(int installmentId)
    {
        return await _context.Installments.FindAsync(installmentId);
    }

    public async Task AddAsync(Installment installment)
    {
        await _context.Installments.AddAsync(installment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Installment installment)
    {
        var installmentToUpdate = await _context.Installments.FindAsync(installment.Id);
        if (installmentToUpdate == null)
        {
            throw new ArgumentNullException(nameof(installmentToUpdate));
        }

        installmentToUpdate.UpdateDetails(
            installment.Amount,
            installment.TermInMonths,
            installment.InterestRate,
            installment.MonthlyPayment,
            installment.StartDate);
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int installmentId)
    {
        var installmentToDelete = await _context.Installments.FindAsync(installmentId);
        if (installmentToDelete == null)
        {
            throw new ArgumentNullException(nameof(installmentToDelete));
        }
        
        _context.Installments.Remove(installmentToDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Installment>> GetInstallmentsByUserIdAsync(int userId)
    {
        return await _context.Installments
            .Where(installment => installment.PayerId == userId)
            .ToListAsync();
    }
}