using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Interfaces;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialSystem.Application.Services;

public class BankService : IBankService
{
    private readonly IBankRepository _bankRepository;
    private readonly IUserRepository _userRepository;
    
    private readonly IUserAccountService _userAccountService;
    private readonly ILoanService _loanService;
    private readonly IInstallmentService _installmentService;
    private readonly ITransferService _transferService;
    private readonly ISalaryProjectService _salaryProjectService;
    
    private readonly ILogger<BankService> _logger;

    public BankService(IBankRepository bankRepository, IUserRepository userRepository,
        IUserAccountService userAccountService, ILoanService loanService,
        IInstallmentService installmentService, ITransferService transferService, ISalaryProjectService salaryProjectService, ILogger<BankService> logger)
    {
        _bankRepository = bankRepository;
        _userRepository = userRepository;
        _userAccountService = userAccountService;
        _loanService = loanService;
        _installmentService = installmentService;
        _transferService = transferService;
        _salaryProjectService = salaryProjectService;
        _logger = logger;
    }

    public async Task RegisterUserToBankAsync(UserBankDto dto)
    {
        _logger.LogInformation($"Starting registration of user {dto.UserId} to bank {dto.BankId} with role {dto.Role}");
        
        var user = await _userRepository.GetByIdAsync(dto.UserId);
        var bank = await _bankRepository.GetByIdAsync(dto.BankId);
        
        if (user == null)
        {
            _logger.LogWarning($"Failed to register user to bank: user with Id {dto.UserId} not found");
            throw new ArgumentNullException($"User with id: {dto.UserId} not found");
        }

        if (bank == null)
        {
            _logger.LogWarning($"Failed to register user to bank: bank with Id {dto.BankId} not found");
            throw new ArgumentNullException($"Bank with id: {dto.BankId} not found");
        }
        
        var userBankRole = new UserBankRole(user, bank, dto.Role);
        await _bankRepository.AddUserToBankAsync(userBankRole);
        
        _logger.LogInformation($"User {dto.UserId} successfully registered to bank {dto.BankId} with role {dto.Role}");
    }

    public async Task<(List<UserBankRole>, List<Bank>)> GetUserBanksAsync(int userId)
    {
        _logger.LogInformation($"Retrieving banks for user {userId}");
        
        var user = await _userRepository.GetByIdWithRolesAsync(userId);
        if (user == null)
        {
            _logger.LogWarning($"Failed to retrieve banks for user: user with Id {userId} not found");
            throw new InvalidOperationException("User not found");
        }

        var registeredBanks = user.UserBankRoles.ToList();

        var allBanks = await _bankRepository.GetAllBanksAsync();
        
        var otherBanks = allBanks.Where(b => registeredBanks.All(r => r.BankId != b.Id)).ToList();
        
        _logger.LogInformation($"Successfully retrieved banks for user {userId}: registered in {registeredBanks.Count}, available {otherBanks.Count} more");
        
        return (registeredBanks, otherBanks);
    }

    public async Task<Bank?> GetBankByIdAsync(int bankId)
    {
        return await _bankRepository.GetByIdAsync(bankId);
    }

    public async Task<IEnumerable<UserAccount>> RetrieveUserAccountsByBankAsync(int userId, int bankId)
    {
        return await _userAccountService.FetchUserAccountsByBankAsync(userId, bankId);
    }

    public async Task<IEnumerable<Loan>> RetrieveUserLoansByBankAsync(int userId, int bankId)
    {
        return await _loanService.FetchUserLoansByBankAsync(userId, bankId);
    }

    public async Task<IEnumerable<Installment>> RetrieveUserInstallmentsByBankAsync(int userId, int bankId)
    {
        return await _installmentService.FetchUserInstallmentsByBankAsync(userId, bankId);
    }

    public async Task<IEnumerable<Loan>> RetrieveLoansByBankAsync(int bankId)
    {
        return await _loanService.FetchLoansByBankAsync(bankId);
    }

    public async Task<IEnumerable<Installment>> RetrieveInstallmentsByBankAsync(int bankId)
    {
        return await _installmentService.FetchInstallmentsByBankAsync(bankId);
    }

    public async Task<IEnumerable<Transfer>> RetrieveTransfersByBankAsync(int bankId)
    {
        return await _transferService.FetchTransfersByBankAsync(bankId);
    }

    public async Task<IEnumerable<SalaryProject>> RetrieveAllSalaryProjectsByBankAsync(int bankId)
    {
        return await _salaryProjectService.FetchAllSalaryProjectsByBankAsync(bankId);
    }

    public async Task<UserAccount?> CreateAccountAsync(UserAccountDto dto)
    {
        return await _userAccountService.CreateUserAccountAsync(dto);
    }

    public async Task CreateLoanAsync(LoanDto dto)
    {
        await _loanService.CreateLoanAsync(dto);
    }

    public async Task CreateInstallmentAsync(InstallmentDto dto)
    {
        await _installmentService.CreateInstallmentAsync(dto);
    }

    public async Task CreateTransferAsync(TransferDto transferDto)
    {
        await _transferService.CreateTransferAsync(transferDto);
    }
}