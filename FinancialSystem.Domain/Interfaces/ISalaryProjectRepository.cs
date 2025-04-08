using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;

namespace FinancialSystem.Domain.Interfaces;

public interface ISalaryProjectRepository
{
    Task<SalaryProject?> GetByIdAsync(int projectId);
    Task<SalaryProject?> GetByIdWithDetailsAsync(int projectId);
    Task AddAsync(SalaryProject salaryProject);
    Task UpdateStatusAsync(int projectId, SalaryProjectStatus status);
    Task DeleteAsync(int projectId);
    Task<IEnumerable<SalaryProject>> GetApprovedSalaryProjectsByBankAsync(int enterpriseId, int bankId);
    Task<IEnumerable<SalaryProject>> GetAllSalaryProjectsByBankAsync(int bankId);
    Task<IEnumerable<SalaryProject>> GetApprovedByEnterpriseIdsAndBankIdAsync(List<int> userEnterpriseIds, int bankId);
}