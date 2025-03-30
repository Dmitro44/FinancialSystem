using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Interfaces;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Interfaces;

namespace FinancialSystem.Application.Services;

public class BankService : IBankService
{
    private readonly IBankRepository _bankRepository;
    private readonly IUserRepository _userRepository;
    
    private readonly IUserAccountService _userAccountService;
    private readonly IEnterpriseAccountService _enterpriseAccountService;
    private readonly ILoanService _loanService;
    private readonly IInstallmentService _installmentService;
    private readonly ITransferService _transferService;
    private readonly ISalaryProjectService _salaryProjectService;

    public BankService(IBankRepository bankRepository, IUserRepository userRepository,
        IUserAccountService userAccountService, ILoanService loanService,
        IInstallmentService installmentService, ITransferService transferService, IEnterpriseAccountService enterpriseAccountService, ISalaryProjectService salaryProjectService)
    {
        _bankRepository = bankRepository;
        _userRepository = userRepository;
        _userAccountService = userAccountService;
        _loanService = loanService;
        _installmentService = installmentService;
        _transferService = transferService;
        _enterpriseAccountService = enterpriseAccountService;
        _salaryProjectService = salaryProjectService;
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

    public async Task CreateAccountAsync(UserAccountDto dto)
    {
        await _userAccountService.CreateAccountAsync(dto);
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