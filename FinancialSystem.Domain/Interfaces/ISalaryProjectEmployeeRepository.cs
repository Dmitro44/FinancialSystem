using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Domain.Interfaces;

public interface ISalaryProjectEmployeeRepository
{
    Task<SalaryProjectEmployee?> GetByIdAsync(int id);
    Task AddAsync(SalaryProjectEmployee salaryProjectEmployee);
    Task UpdateAsync(SalaryProjectEmployee salaryProjectEmployee);
    Task DeleteAsync(int salaryProjectEmployeeId);
    Task<SalaryProjectEmployee?> GetProjectByUserAndProjectIdAsync(int userId, int salaryProjectId);
    Task<IEnumerable<SalaryProjectEmployee>> GetByUserAndBankIdAsync(int userId, int bankId);
    Task<IEnumerable<SalaryProjectEmployee>> GetByProjectIdAsync(int projectId);
    Task<IEnumerable<SalaryProjectEmployee>> GetArchivedByProjectIdAsync(int salaryProjectId);
    Task RestoreAsync(int employeeId);
}