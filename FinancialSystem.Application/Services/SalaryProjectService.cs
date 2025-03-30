using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Interfaces;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;
using FinancialSystem.Domain.Interfaces;

namespace FinancialSystem.Application.Services;

public class SalaryProjectService : ISalaryProjectService
{
    private readonly IEnterpriseRepository _enterpriseRepository;
    private readonly IEnterpriseAccountRepository _enterpriseAccountRepository;
    private readonly ISalaryProjectRepository _salaryProjectRepository;
    private readonly IBankRepository _bankRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserEnterpriseRepository _userEnterpriseRepository;
    private readonly ISalaryProjectEmployeeRepository _salaryProjectEmployeeRepository;

    public SalaryProjectService(IEnterpriseRepository enterpriseRepository, IEnterpriseAccountRepository enterpriseAccountRepository, ISalaryProjectRepository salaryProjectRepository, IBankRepository bankRepository, IUserRepository userRepository, IUserEnterpriseRepository userEnterpriseRepository, ISalaryProjectEmployeeRepository salaryProjectEmployeeRepository)
    {
        _enterpriseRepository = enterpriseRepository;
        _enterpriseAccountRepository = enterpriseAccountRepository;
        _salaryProjectRepository = salaryProjectRepository;
        _bankRepository = bankRepository;
        _userRepository = userRepository;
        _userEnterpriseRepository = userEnterpriseRepository;
        _salaryProjectEmployeeRepository = salaryProjectEmployeeRepository;
    }

    public async Task CreateSalaryProjectAsync(SalaryProjectDto dto)
    {
        var enterprise = await _enterpriseRepository.GetByIdAsync(dto.EnterpriseId)
            ?? throw new ApplicationException($"Enterprise with id: {dto.EnterpriseId} does not exist.");

        var enterpriseAccount = await _enterpriseAccountRepository.GetByIdAsync(dto.EnterpriseAccountId)
            ?? throw new ApplicationException($"Enterprise Account with id: {dto.EnterpriseAccountId} does not exist.");

        var bank = await _bankRepository.GetByIdAsync(dto.BankId)
            ?? throw new ApplicationException($"Bank with id: {dto.BankId} does not exist.");
        
        var salaryProject = new SalaryProject(enterprise, enterpriseAccount, dto.Salary, bank);
        
        await _salaryProjectRepository.AddAsync(salaryProject);
    }

    public async Task<IEnumerable<SalaryProject>> FetchApprovedSalaryProjectsByBankAsync(int enterpriseId, int bankId)
    {
        return await _salaryProjectRepository.GetApprovedSalaryProjectsByBankAsync(enterpriseId, bankId);
    }

    public async Task<IEnumerable<SalaryProject>> FetchAllSalaryProjectsByBankAsync(int bankId)
    {
        return await _salaryProjectRepository.GetAllSalaryProjectsByBankAsync(bankId);
    }

    public async Task UpdateStatusAsync(int salaryProjectId, SalaryProjectStatus status)
    {
        await _salaryProjectRepository.UpdateStatusAsync(salaryProjectId, status);
    }

    public async Task ConnectUserToSalaryProject(int userId, int salaryProjectId)
    {
        var salaryProject = await _salaryProjectRepository.GetByIdAsync(salaryProjectId);
        if (salaryProject == null || salaryProject.Status != SalaryProjectStatus.Approved)
        {
            throw new ArgumentException("Salary project not found or not approved.");
        }
        
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException("User not found.");
        }

        var userEnterprise = await _userEnterpriseRepository.GetByUserAndEnterpriseId(userId, salaryProject.EnterpriseId);
        if (userEnterprise == null)
        {
            throw new InvalidOperationException("User is not an active employee of this enterprise.");
        }
        
        var existingConnection = await _salaryProjectEmployeeRepository.GetProjectByUserAndProjectIdAsync(userId, salaryProjectId);
        if (existingConnection != null)
        {
            throw new InvalidOperationException("User is already connected to this salary project.");
        }

        var salaryProjectEmployee = new SalaryProjectEmployee(salaryProject, user);
        await _salaryProjectEmployeeRepository.AddAsync(salaryProjectEmployee);
    }

    public async Task<List<SalaryProject>> GetAvailableSalaryProjectsForUserAsync(int userId, int bankId)
    {
        var userEnterpriseIds = (await _userEnterpriseRepository
            .GetByUserIdAsync(userId))
            .Select(ue => ue.EnterpriseId)
            .ToList();

        if (!userEnterpriseIds.Any())
        {
            return new List<SalaryProject>();
        }
        
        var salaryProjects = await _salaryProjectRepository.GetApprovedByEnterpriseIdsAndBankIdAsync(userEnterpriseIds, bankId);
        
        var connectedProjectIds = (await _salaryProjectEmployeeRepository
            .GetByUserIdAsync(userId))
            .Select(spe => spe.SalaryProjectId)
            .ToList();
        
        var availableProjects = salaryProjects
            .Where(sp => !connectedProjectIds.Contains(sp.Id))
            .ToList();
        
        return availableProjects;
    }

    public async Task<List<SalaryProjectEmployee>> GetUserSalaryProjectsAsync(int userId)
    {
        var connections = await _salaryProjectEmployeeRepository.GetByUserIdAsync(userId);

        return connections.ToList();
    }
}