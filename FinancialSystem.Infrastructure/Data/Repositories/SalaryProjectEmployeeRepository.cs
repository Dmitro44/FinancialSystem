using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialSystem.Infrastructure.Data.Repositories;

public class SalaryProjectEmployeeRepository : ISalaryProjectEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public SalaryProjectEmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SalaryProjectEmployee?> GetByIdAsync(int id)
    {
        return await _context.SalaryProjectEmployees.FindAsync(id);
    }

    public async Task AddAsync(SalaryProjectEmployee salaryProjectEmployee)
    {
        await _context.SalaryProjectEmployees.AddAsync(salaryProjectEmployee);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(SalaryProjectEmployee salaryProjectEmployee)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(int salaryProjectEmployeeId)
    {
        var spEmployeeToUpdate = await _context.SalaryProjectEmployees.FindAsync(salaryProjectEmployeeId);
        if (spEmployeeToUpdate == null)
        {
            throw new ArgumentNullException(nameof(spEmployeeToUpdate));
        }
        
        _context.SalaryProjectEmployees.Remove(spEmployeeToUpdate);
        await _context.SaveChangesAsync();
    }

    public async Task<SalaryProjectEmployee?> GetProjectByUserAndProjectIdAsync(int userId, int salaryProjectId)
    {
        return await _context.SalaryProjectEmployees
            .FirstOrDefaultAsync(spe => spe.UserId == userId && spe.SalaryProjectId == salaryProjectId);
    }

    public async Task<IEnumerable<SalaryProjectEmployee>> GetByUserIdAsync(int userId)
    {
        return await _context.SalaryProjectEmployees
            .Where(spe => spe.UserId == userId)
            .ToListAsync();
    }
}