using FinancialSystem.Application.DTOs;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Interfaces;

namespace FinancialSystem.Application.Services;

public class BankService
{
    private readonly IBankRepository _bankRepository;
    private readonly IUserRepository _userRepository;
    
    private readonly AccountService _accountService;
    private readonly LoanService _loanService;
    private readonly InstallmentService _installmentService;

    public BankService(IBankRepository bankRepository, IUserRepository userRepository,
        AccountService accountService, LoanService loanService,
        InstallmentService installmentService)
    {
        _bankRepository = bankRepository;
        _userRepository = userRepository;
        _accountService = accountService;
        _loanService = loanService;
        _installmentService = installmentService;
    }

    public async Task RegisterUserToBankAsync(UserBankDto dto)
    {
        var user = await _userRepository.GetByIdAsync(dto.UserId);
        var bank = await _bankRepository.GetByIdAsync(dto.BankId);

        if (user == null)
        {
            throw new ArgumentNullException($"User with id: {dto.UserId} not found");
        }

        if (bank == null)
        {
            throw new ArgumentNullException($"Bank with id: {dto.BankId} not found");
        }
        
        var userBankRole = new UserBankRole(user, bank, dto.Role);
        await _bankRepository.AddUserToBankAsync(userBankRole);
    }

    public async Task<(List<UserBankRole>, List<Bank>)> GetUserBanksAsync(int userId)
    {
        var user = await _userRepository.GetByIdWithRolesAsync(userId);
        if (user == null)
            throw new InvalidOperationException("User not found");

        var registeredBanks = user.UserBankRoles.ToList();

        var allBanks = await _bankRepository.GetAllBanksAsync();
        
        var otherBanks = allBanks.Where(b => registeredBanks.All(r => r.BankId != b.Id)).ToList();
        
        return (registeredBanks, otherBanks);
    }

    public async Task<Bank?> GetBankByIdAsync(int bankId)
    {
        return await _bankRepository.GetByIdAsync(bankId);
    }

    public async Task<IEnumerable<Account>> GetAccountsForUserAsync(int userId)
    {
        return await _accountService.GetAccountsByUserIdAsync(userId);
    }

    public async Task<IEnumerable<Loan>> GetLoansForUserAsync(int userId)
    {
        return await _loanService.GetLoansByUserIdAsync(userId);
    }

    public async Task<IEnumerable<Installment>> GetInstallmentsForUserAsync(int userId)
    {
        return await _installmentService.GetInstallmentsByUserIdAsync(userId);
    }

    public async Task CreateAccountAsync(AccountDto dto)
    {
        await _accountService.CreateAccountAsync(dto);
    }

    public async Task CreateLoanAsync(LoanDto dto)
    {
        await _loanService.CreateLoanAsync(dto);
    }

    public async Task CreateInstallmentAsync(InstallmentDto dto)
    {
        await _installmentService.CreateInstallmentAsync(dto);
    }
}