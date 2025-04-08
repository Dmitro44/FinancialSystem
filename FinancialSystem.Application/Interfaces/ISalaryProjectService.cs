using FinancialSystem.Application.DTOs;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;

namespace FinancialSystem.Application.Interfaces;

public interface ISalaryProjectService
{
    Task CreateSalaryProjectAsync(SalaryProjectDto dto);
    Task<IEnumerable<SalaryProject>> FetchApprovedSalaryProjectsByBankAsync(int enterpriseId, int bankId);
    Task<IEnumerable<SalaryProject>> FetchAllSalaryProjectsByBankAsync(int bankId);
    Task UpdateStatusAsync(int salaryProjectId, SalaryProjectStatus status);
    Task<List<SalaryProject>> GetAvailableSalaryProjectsForUserAsync(int userId, int bankId);
    Task ConnectUserToSalaryProject(int userId, int salaryProjectId);
    Task<(bool success, string message)> DisconnectUserFromSalaryProject(int userId, int salaryProjectId);
    Task<List<SalaryProjectEmployee>> GetUserSalaryProjectsAsync(int userId, int bankId);
    Task<SalaryPaymentResultDto> ProcessSalaryPaymentsAsync(int salaryProjectId);
}