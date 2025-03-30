using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Domain.Interfaces;

public interface ISalaryProjectEmployeeRepository
{
    Task<SalaryProjectEmployee?> GetByIdAsync(int id);
    Task AddAsync(SalaryProjectEmployee salaryProjectEmployee);
    Task UpdateAsync(SalaryProjectEmployee salaryProjectEmployee);
    Task DeleteAsync(int salaryProjectEmployeeId);
    Task<SalaryProjectEmployee?> GetProjectByUserAndProjectIdAsync(int userId, int salaryProjectId);
    Task<IEnumerable<SalaryProjectEmployee>> GetByUserIdAsync(int userId);
}