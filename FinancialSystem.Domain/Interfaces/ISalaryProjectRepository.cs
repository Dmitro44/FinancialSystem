using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Domain.Interfaces;

public interface ISalaryProjectRepository
{
    Task<SalaryProject?> GetByIdAsync(int projectId);
    Task AddAsync(SalaryProject salaryProject);
    Task DeleteAsync(int projectId);
}