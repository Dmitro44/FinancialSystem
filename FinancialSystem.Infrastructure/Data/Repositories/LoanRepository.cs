using FinancialSystem.Application.DTOs;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Loan loan)
    {
        var loanToUpdate = await _context.Loans.FindAsync(loan.Id);
        if (loanToUpdate == null)
        {
            throw new ArgumentNullException(nameof(loanToUpdate));
        }
        
        loanToUpdate.UpdateDetails(
            loan.Amount,
            loan.TermInMonths,
            loan.InterestRate,
            loan.TotalAmount,
            loan.MonthlyPayment,
            loan.StartDate);

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
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Loan>> GetLoansByUserIdAsync(int userId)
    {
        return await _context.Loans
            .Where(loan => loan.BorrowerId == userId)
            .ToListAsync();
    }
}