using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Interfaces;

namespace FinancialSystem.Infrastructure.Data.Repositories;

public class LoanRepository : ILoanRepository
{
    private readonly ApplicationDbContext _context;

    public LoanRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Loan?> GetByIdAsync(int loanId)
    {
        return await _context.Loans.FindAsync(loanId);
    }

    public async Task AddAsync(Loan loan)
    {
        await _context.Loans.AddAsync(loan);
    }

    public async Task UpdateAsync(Loan loan)
    {
        var loanToUpdate = await _context.Loans.FindAsync(loan.Id);
        if (loanToUpdate == null)
        {
            throw new ArgumentNullException(nameof(loanToUpdate));
        }
        
        _context.Loans.Update(loanToUpdate);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int loanId)
    {
        var loanToDelete = await _context.Loans.FindAsync(loanId);
        if (loanToDelete == null)
        {
            throw new ArgumentNullException(nameof(loanToDelete));
        }

        _context.Loans.Remove(loanToDelete);
    }
}