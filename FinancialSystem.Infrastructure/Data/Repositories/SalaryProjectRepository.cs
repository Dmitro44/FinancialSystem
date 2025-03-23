using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Interfaces;

namespace FinancialSystem.Infrastructure.Data.Repositories;

public class SalaryProjectRepository : ISalaryProjectRepository
{
    private readonly ApplicationDbContext _context;

    public SalaryProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SalaryProject?> GetByIdAsync(int projectId)
    {
        return await _context.SalaryProjects.FindAsync(projectId);
    }

    public async Task AddAsync(SalaryProject salaryProject)
    {
        await _context.SalaryProjects.AddAsync(salaryProject);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int projectId)
    {
        var projectToDelete = await _context.SalaryProjects.FindAsync(projectId);
        if (projectToDelete == null)
        {
            throw new ArgumentNullException(nameof(projectToDelete));
        }
        
        _context.SalaryProjects.Remove(projectToDelete);
        await _context.SaveChangesAsync();
    }
}