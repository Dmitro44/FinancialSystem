using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Interfaces;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;
using FinancialSystem.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialSystem.Application.Services;

public class SalaryProjectService : ISalaryProjectService
{
    private readonly IEnterpriseRepository _enterpriseRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ISalaryProjectRepository _salaryProjectRepository;
    private readonly IBankRepository _bankRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserEnterpriseRepository _userEnterpriseRepository;
    private readonly ISalaryProjectEmployeeRepository _salaryProjectEmployeeRepository;
    private readonly ITransferService _transferService;
    private readonly ILogger<SalaryProjectService> _logger;
    
    private readonly IUserAccountService _userAccountService;

    public SalaryProjectService(IEnterpriseRepository enterpriseRepository, IAccountRepository accountRepository, ISalaryProjectRepository salaryProjectRepository,
        IBankRepository bankRepository, IUserRepository userRepository, IUserEnterpriseRepository userEnterpriseRepository, 
        ISalaryProjectEmployeeRepository salaryProjectEmployeeRepository, IUserAccountService userAccountService, ITransferService transferService, ILogger<SalaryProjectService> logger)
    {
        _enterpriseRepository = enterpriseRepository;
        _accountRepository = accountRepository;
        _salaryProjectRepository = salaryProjectRepository;
        _bankRepository = bankRepository;
        _userRepository = userRepository;
        _userEnterpriseRepository = userEnterpriseRepository;
        _salaryProjectEmployeeRepository = salaryProjectEmployeeRepository;
        _userAccountService = userAccountService;
        _transferService = transferService;
        _logger = logger;
    }

    public async Task CreateSalaryProjectAsync(SalaryProjectDto dto)
    {
        _logger.LogInformation("Starting creation of salary project for enterprise {EnterpriseId} in bank {BankId} with salary {Salary}",
            dto.EnterpriseId, dto.BankId, dto.Salary);

        try
        {
            var enterprise = await _enterpriseRepository.GetByIdAsync(dto.EnterpriseId);
            if (enterprise == null)
            {
                _logger.LogWarning("Failed to create salary project: enterprise with ID {EnterpriseId} not found", dto.EnterpriseId);
                throw new ApplicationException($"Enterprise with id: {dto.EnterpriseId} does not exist.");
            }

            var enterpriseAccount = await _accountRepository.GetEnterpriseAccountByIdAsync(dto.EnterpriseAccountId);
            if (enterpriseAccount == null)
            {
                _logger.LogWarning("Failed to create salary project: enterprise account with ID {EnterpriseAccountId} not found", dto.EnterpriseAccountId);
                throw new ApplicationException($"Enterprise Account with id: {dto.EnterpriseAccountId} does not exist.");
            }

            var bank = await _bankRepository.GetByIdAsync(dto.BankId);
            if (bank == null)
            {
                _logger.LogWarning("Failed to create salary project: bank with ID {BankId} not found", dto.BankId);
                throw new ApplicationException($"Bank with id: {dto.BankId} does not exist.");
            }
        
            var salaryProject = new SalaryProject(enterprise, enterpriseAccount, dto.Salary, bank);
        
            await _salaryProjectRepository.AddAsync(salaryProject);
            
            _logger.LogInformation("Salary project successfully created for enterprise {EnterpriseId} in bank {BankId} with ID {SalaryProjectId}",
                dto.EnterpriseId, dto.BankId, salaryProject.Id);
        }
        catch (Exception e) when (!(e is ApplicationException))
        {
            _logger.LogError(e, "Error creating salary project for enterprise {EnterpriseId} in bank {BankId}", dto.EnterpriseId, dto.BankId);
            throw;
        }
    }

    public async Task<IEnumerable<SalaryProject>> FetchApprovedSalaryProjectsByBankAsync(int enterpriseId, int bankId)
    {
        _logger.LogInformation("Fetching approved salary projects for enterprise {EnterpriseId} in bank {BankId}",
            enterpriseId, bankId);

        try
        {
            var projects = await _salaryProjectRepository.GetApprovedSalaryProjectsByBankAsync(enterpriseId, bankId);
            
            _logger.LogInformation("Successfully fetched approved salary projects for enterprise {EnterpriseId} in bank {BankId}",
                enterpriseId, bankId);
            
            return projects;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching approved salary projects for enterprise {EnterpriseId} in bank {BankId}",
                enterpriseId, bankId);
            throw;
        }
    }

    public async Task<IEnumerable<SalaryProject>> FetchAllSalaryProjectsByBankAsync(int bankId)
    {
        _logger.LogInformation("Fetching all salary projects in bank {BankId}", bankId);

        try
        {
            var projects = await _salaryProjectRepository.GetAllSalaryProjectsByBankAsync(bankId);
            
            _logger.LogInformation("Successfully fetched all salary projects in bank {BankId}", bankId);
            
            return projects;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching all salary projects in bank {BankId}", bankId);
            throw;
        }
    }

    public async Task UpdateStatusAsync(int salaryProjectId, SalaryProjectStatus status)
    {
        _logger.LogInformation("Updating status of salary project {SalaryProjectId} to {NewStatus}", 
            salaryProjectId, status);
        
        try
        {
            await _salaryProjectRepository.UpdateStatusAsync(salaryProjectId, status);
            
            _logger.LogInformation("Successfully updated status of salary project {SalaryProjectId} to {NewStatus}", 
                salaryProjectId, status);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating status of salary project {SalaryProjectId} to {NewStatus}", 
                salaryProjectId, status);
            throw;
        }
    }

    public async Task ConnectUserToSalaryProject(int userId, int salaryProjectId)
    {
        _logger.LogInformation("Connecting user {UserId} to salary project {SalaryProjectId}", userId, salaryProjectId);

        try
        {
            var salaryProject = await _salaryProjectRepository.GetByIdAsync(salaryProjectId);
            if (salaryProject == null || salaryProject.Status != SalaryProjectStatus.Approved)
            {
                _logger.LogWarning("Failed to connect user to salary project: project {SalaryProjectId} not found or not approved",
                    salaryProjectId);
                throw new ArgumentException("Salary project not found or not approved.");
            }
        
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Failed to connect user to salary project: user {UserId} not found",
                    userId);
                throw new ArgumentException("User not found.");
            }

            var userEnterprise = await _userEnterpriseRepository.GetByUserAndEnterpriseId(userId, salaryProject.EnterpriseId);
            if (userEnterprise == null)
            {
                _logger.LogWarning("Failed to connect user to salary project: user {UserId} is not an employee of enterprise {EnterpriseId}",
                    userId, salaryProject.EnterpriseId);
                throw new InvalidOperationException("User is not an active employee of this enterprise.");
            }
        
            var existingConnection = await _salaryProjectEmployeeRepository.GetProjectByUserAndProjectIdAsync(userId, salaryProjectId);
            if (existingConnection != null)
            {
                _logger.LogWarning("Failed to connect user to salary project: user {UserId} is already connected to project {SalaryProjectId}",
                    userId, salaryProjectId);
                throw new InvalidOperationException("User is already connected to this salary project.");
            }

            var inactiveAccount =
                await _accountRepository.GetInactiveAccountBySalaryProjectAsync(userId, salaryProjectId);

            UserAccount? userAccount;
            
            if (inactiveAccount == null)
            {
                var userAccountDto = new UserAccountDto
                {
                    OwnerId = userId,
                    BankId = salaryProject.BankId,
                    Balance = 0,
                    AccountType = AccountType.Salary,
                    EmployerEnterprise = userEnterprise.Enterprise
                };
        
                userAccount = await _userAccountService.CreateUserAccountAsync(userAccountDto);
                
                _logger.LogInformation("Created new salary account {AccountId} for user {UserId}",
                    userAccount.Id, userId);
            }
            else
            {
                inactiveAccount.Activate();
                
                await _accountRepository.UpdateAsync(inactiveAccount);

                userAccount = inactiveAccount;
                
                _logger.LogInformation("Reactivated salary account {AccountId} for user {UserId}",
                    userAccount.Id, userId);
            }
            
            var salaryProjectEmployee = new SalaryProjectEmployee(salaryProject, user, userAccount);
            await _salaryProjectEmployeeRepository.AddAsync(salaryProjectEmployee);
            
            _logger.LogInformation("Successfully connected user {UserId} to salary project {SalaryProjectId} with account {AccountId}",
                userId, salaryProjectId, userAccount.Id);
        }
        catch (Exception e) when (!(e is ArgumentException || e is InvalidOperationException))
        {
            _logger.LogError(e, "Error connecting user {UserId} to salary project {SalaryProjectId}", userId, salaryProjectId);
            throw;
        }
    }

    public async Task<(bool success, string message)> DisconnectUserFromSalaryProject(int userId, int salaryProjectId)
    {
        _logger.LogInformation("Disconnecting user {UserId} from salary project {SalaryProjectId}",
            userId, salaryProjectId);

        var existingConnection =
            await _salaryProjectEmployeeRepository.GetProjectByUserAndProjectIdAsync(userId, salaryProjectId);
        if (existingConnection == null)
        {
            _logger.LogWarning("Failed to disconnect from salary project: user {UserId} is not connected to project {SalaryProjectId}",
                userId, salaryProjectId);
            return (false, $"User {userId} is not connected to this salary project.");
        }

        var userAccount = existingConnection.UserAccount;

        if (userAccount.Balance > 0)
        {
            _logger.LogWarning("Cannot delete salary user account {AccountId} with positive balance {Balance}",
                userAccount.Id, userAccount.Balance);
            return (false, $"Cannot delete account with positive balance ({userAccount.Balance}). Please transfer the funds first");
        }
        
        await _salaryProjectEmployeeRepository.DeleteAsync(existingConnection.Id);

        userAccount.Deactivate();
        
        await _accountRepository.UpdateAsync(userAccount);
        
        _logger.LogInformation("Successfully disconnected user {UserId} from salary project {SalaryProjectId} and deleted associated account",
            userId, salaryProjectId);

        return (true, "Successfully disconnected from salary project and deleted the associated account.");
    }
    
    public async Task<List<SalaryProject>> GetAvailableSalaryProjectsForUserAsync(int userId, int bankId)
    {
        _logger.LogInformation("Fetching available salary projects for user {UserId} in bank {BankId}",
            userId, bankId);
        
        try
        {
            var userEnterpriseIds = (await _userEnterpriseRepository
                    .GetByUserIdAsync(userId))
                .Select(ue => ue.EnterpriseId)
                .ToList();

            if (!userEnterpriseIds.Any())
            {
                _logger.LogInformation("No enterprises found for user {UserId} in bank {BankId}",
                    userId, bankId);
                return new List<SalaryProject>();
            }
        
            var salaryProjects = await _salaryProjectRepository.GetApprovedByEnterpriseIdsAndBankIdAsync(userEnterpriseIds, bankId);
        
            var connectedProjectIds = (await _salaryProjectEmployeeRepository
                    .GetByUserAndBankIdAsync(userId, bankId))
                .Select(spe => spe.SalaryProjectId)
                .ToList();
        
            var availableProjects = salaryProjects
                .Where(sp => !connectedProjectIds.Contains(sp.Id))
                .ToList();
        
            _logger.LogInformation("Found {Count} available salary projects for user {UserId} in bank {BankId}",
                availableProjects.Count, userId, bankId);
            
            return availableProjects;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching available salary projects for user {UserId} in bank {BankId}",
                userId, bankId);
            throw;
        }
    }

    public async Task<List<SalaryProjectEmployee>> GetUserSalaryProjectsAsync(int userId, int bankId)
    {
        _logger.LogInformation("Fetching salary projects for user {UserId}", userId);

        try
        {
            var connections = await _salaryProjectEmployeeRepository.GetByUserAndBankIdAsync(userId, bankId);
            
            _logger.LogInformation("Successfully fetched salary projects for user {UserId}", userId);

            return connections.ToList();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching salary projects for user {UserId}", userId);
            throw;
        }
    }

    public async Task<SalaryPaymentResultDto> ProcessSalaryPaymentsAsync(int salaryProjectId)
    {
        _logger.LogInformation("Starting salary payment processing for project {SalaryProjectId}",
            salaryProjectId);
        
        var salaryProject = await _salaryProjectRepository.GetByIdWithDetailsAsync(salaryProjectId);
        if (salaryProject == null)
        {
            _logger.LogWarning("Failed to process salary payments: project {SalaryProjectId} not found",
                salaryProjectId);
            throw new ArgumentException("Salary project not found.", nameof(salaryProjectId));
        }

        if (salaryProject.Status != SalaryProjectStatus.Approved)
        {
            _logger.LogWarning("Failed to process salary payments: project {SalaryProjectId} is not approved", 
                salaryProjectId);
            throw new InvalidOperationException("Cannot process payments for non-approved salary project");
        }

        var enterpriseAccount = salaryProject.EnterpriseAccount;
        if (enterpriseAccount == null)
        {
            _logger.LogWarning("Failed to process salary payments: enterprise account not found for project {SalaryProjectId}",
                salaryProjectId);
            throw new InvalidOperationException("Enterprise account not found.");
        }

        var employees = (await _salaryProjectEmployeeRepository.GetByProjectIdAsync(salaryProjectId)).ToList();
        if (!employees.Any())
        {
            _logger.LogInformation("No employees connected to salary project {SalaryProjectId}, skipping payment processing", 
                salaryProjectId);
            
            return new SalaryPaymentResultDto
            {
                Success = true,
                Message = "No employees connected to this salary project",
                SuccessfulPayments = 0,
                FailedPayments = 0
            };
        }

        decimal totalAmount = salaryProject.Salary * employees.Count;

        if (enterpriseAccount.Balance < totalAmount)
        {
            _logger.LogWarning("Insufficient funds for salary payments: required {RequiredAmount}, available {AvailableAmount}", 
                totalAmount, enterpriseAccount.Balance);
            
            throw new InvalidOperationException($"Insufficient funds in enterprise account. Required: {totalAmount}, Available: {enterpriseAccount.Balance}");
        }

        var result = new SalaryPaymentResultDto
        {
            TotalAmount = totalAmount,
            EmployeesCount = employees.Count
        };

        var successfulPayments = 0;
        var failedPayments = 0;
        var errors = new List<string>();
        
        _logger.LogInformation("Processing salary payments for {Count} employees in project {SalaryProjectId}", 
            employees.Count, salaryProjectId);

        foreach (var employee in employees)
        {
            try
            {
                var userAccount = employee.UserAccount;
                if (userAccount == null)
                {
                    _logger.LogWarning("Failed to process salary payment: user account not found for employee {UserId}", 
                        employee.UserId);
                    
                    failedPayments++;
                    errors.Add($"User account for employee {employee.UserId.ToString()} is not active or not found");
                    continue;
                }

                var transferDto = new TransferDto
                {
                    SenderId = enterpriseAccount.Id,
                    ReceiverId = userAccount.Id,
                    Amount = salaryProject.Salary,
                    Status = TransferStatus.Active,
                    Type = TransferType.Salary
                };

                await _transferService.CreateTransferAsync(transferDto);

                successfulPayments++;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error processing salary payment for employee {UserId}: {ErrorMessage}", 
                    employee.UserId, e.Message);
                
                failedPayments++;
                errors.Add($"Error processing payment for employee {employee.UserId.ToString()}: {e.Message}");
            }
        }
        
        result.Success = failedPayments == 0;
        result.SuccessfulPayments = successfulPayments;
        result.FailedPayments = failedPayments;
        result.Message = failedPayments == 0 
            ? "All salary payments were processed successfully" 
            : $"{successfulPayments} payments processed successfully, {failedPayments} payments failed";
        result.Errors = errors;

        _logger.LogInformation("Completed salary payment processing for project {SalaryProjectId}: {SuccessfulCount} successful, {FailedCount} failed", 
            salaryProjectId, successfulPayments, failedPayments);
        
        if (failedPayments > 0)
        {
            _logger.LogWarning("Some salary payments failed for project {SalaryProjectId}. Errors: {Errors}", 
                salaryProjectId, string.Join("; ", errors));
        }
        
        return result;
    }

    public async Task<bool> RevertSalaryProjectCreationAsync(int salaryProjectId)
    {
        _logger.LogInformation("Reverting salary project {ProjectId} creation",
            salaryProjectId);

        try
        {
            var salaryProject = await _salaryProjectRepository.GetByIdAsync(salaryProjectId);
            if (salaryProject == null || salaryProject.Status != SalaryProjectStatus.Approved)
            {
                _logger.LogWarning("Cannot revert salary project creation: project {ProjectId} not found or not approved",
                    salaryProjectId);
                return false;
            }

            var salaryProjectEmployee = await _salaryProjectEmployeeRepository.GetByProjectIdAsync(salaryProjectId);
            foreach (var employee in salaryProjectEmployee)
            {
                var userAccount = await _accountRepository.GetByIdAsync(employee.UserAccountId);
                if (userAccount != null)
                {
                    userAccount.Deactivate();
                    await _accountRepository.UpdateAsync(userAccount);
                }

                employee.Deactivate();
                await _salaryProjectEmployeeRepository.UpdateAsync(employee);
            }
            
            salaryProject.Deactivate();
            await _salaryProjectRepository.UpdateAsync(salaryProject);
        
            _logger.LogInformation("Successfully reverted salary project {ProjectId} creation",
                salaryProjectId);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reverting salary project {ProjectId} creation",
                salaryProjectId);
            return false;
        }
    }

    public async Task<bool> RestoreSalaryProjectCreationAsync(int salaryProjectId)
    {
        _logger.LogInformation("Restoring salary project {ProjectId} creation",
            salaryProjectId);

        try
        {
            var salaryProject = await _salaryProjectRepository.GetByIdAsync(salaryProjectId);
            if (salaryProject == null)
            {
                _logger.LogWarning("Cannot restore salary project: project {ProjectId} not found",
                    salaryProjectId);
                return false;
            }

            if (salaryProject.IsActive)
            {
                _logger.LogWarning("Salary project {ProjectId} is already active",
                    salaryProjectId);
                return true;
            }
        
            salaryProject.Activate();
            await _salaryProjectRepository.UpdateAsync(salaryProject);
        
            var archivedEmployees = await _salaryProjectEmployeeRepository.GetArchivedByProjectIdAsync(salaryProjectId);

            foreach (var employee in archivedEmployees)
            {
                await _salaryProjectEmployeeRepository.RestoreAsync(employee.Id);
            
                var userAccount = await _accountRepository.GetByIdAsync(employee.UserAccountId);
                if (userAccount != null)
                {
                    userAccount.Activate();
                    await _accountRepository.UpdateAsync(userAccount);
                
                    _logger.LogInformation("Activated salary account {AccountId} for employee {EmployeeId} in salary project {ProjectId}",
                        userAccount.Id, employee.UserId, salaryProject.Id);
                }
            }
        
            _logger.LogInformation("Successfully restored salary project {ProjectId}",
                salaryProject.Id);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error restoring salary project {ProjectId}",
                salaryProjectId);
            return false;
        }
    }
}