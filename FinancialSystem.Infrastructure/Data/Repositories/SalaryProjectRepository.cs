using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;
using FinancialSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

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

    public async Task<SalaryProject?> GetByIdWithDetailsAsync(int projectId)
    {
        return await _context.SalaryProjects
            .Include(sp => sp.EnterpriseAccount)
            .FirstOrDefaultAsync(sp => sp.Id == projectId);
    }

    public async Task AddAsync(SalaryProject salaryProject)
    {
        await _context.SalaryProjects.AddAsync(salaryProject);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(int projectId, SalaryProjectStatus status)
    {
        var projectToUpdate = await _context.SalaryProjects.FindAsync(projectId);
        if (projectToUpdate == null)
        {
            throw new ArgumentNullException(nameof(projectToUpdate));
        }
        
        projectToUpdate.SetStatus(status);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(SalaryProject salaryProject)
    {
        var projectToUpdate = await _context.SalaryProjects.FindAsync(salaryProject.Id);
        if (projectToUpdate == null)
        {
            throw new ArgumentNullException(nameof(projectToUpdate));
        }
        
        projectToUpdate.UpdateDetails(salaryProject.Salary);
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

    public async Task<IEnumerable<SalaryProject>> GetApprovedSalaryProjectsByBankAsync(int enterpriseId, int bankId)
    {
        return await _context.SalaryProjects
            .Where(sp => sp.EnterpriseId == enterpriseId && sp.BankId == bankId && sp.Status == SalaryProjectStatus.Approved && sp.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<SalaryProject>> GetAllSalaryProjectsByBankAsync(int bankId)
    {
        return await _context.SalaryProjects
            .Include(sp => sp.Enterprise)
            .Include(sp => sp.EnterpriseAccount)
            .Where(sp => sp.BankId == bankId)
            .ToListAsync();
    }
    

    public async Task<IEnumerable<SalaryProject>> GetApprovedByEnterpriseIdsAndBankIdAsync(List<int> userEnterpriseIds, int bankId)
    {
        return await _context.SalaryProjects
            .Include(sp => sp.Enterprise)
            .Include(sp => sp.EnterpriseAccount)
                .ThenInclude(ea => ea.Bank)
            .Where(sp =>
                userEnterpriseIds.Contains(sp.EnterpriseId) &&
                sp.Status == SalaryProjectStatus.Approved &&
                sp.BankId == bankId &&
                sp.IsActive)
            .ToListAsync();
    }
}