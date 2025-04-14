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
        var speToUpdate = await _context.SalaryProjectEmployees.FindAsync(salaryProjectEmployee.Id);
        if (speToUpdate == null)
        {
            throw new ArgumentNullException(nameof(speToUpdate));
        }
        
        await _context.SaveChangesAsync();
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
            .Include(spe => spe.UserAccount)
            .FirstOrDefaultAsync(spe => spe.UserId == userId && spe.SalaryProjectId == salaryProjectId);
    }

    public async Task<IEnumerable<SalaryProjectEmployee>> GetByUserAndBankIdAsync(int userId, int bankId)
    {
        return await _context.SalaryProjectEmployees
            .Include(spe => spe.SalaryProject)
                .ThenInclude(s => s.Enterprise)
            .Include(spe => spe.SalaryProject)
                .ThenInclude(s => s.Bank)
            .Where(spe => spe.UserId == userId && spe.SalaryProject.BankId == bankId && spe.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<SalaryProjectEmployee>> GetByProjectIdAsync(int projectId)
    {
        return await _context.SalaryProjectEmployees
            .Include(spe => spe.UserAccount)
            .Where(spe => spe.SalaryProjectId == projectId && spe.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<SalaryProjectEmployee>> GetArchivedByProjectIdAsync(int salaryProjectId)
    {
        return await _context.SalaryProjectEmployees
            .Where(spe => spe.SalaryProjectId == salaryProjectId && !spe.IsActive)
            .ToListAsync();
    }

    public async Task RestoreAsync(int employeeId)
    {
        var employee = await _context.SalaryProjectEmployees.FindAsync(employeeId);
        if (employee == null)
        {
            throw new ArgumentNullException(nameof(employee));
        }
        
        employee.Activate();
        await _context.SaveChangesAsync();
    }
}