using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Interfaces;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;
using FinancialSystem.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using ApplicationException = System.ApplicationException;

namespace FinancialSystem.Application.Services;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _loanRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBankRepository _bankRepository;
    private readonly ITransferService _transferService;
    private readonly IUserAccountService _userAccountService;
    private readonly IAccountRepository _accountRepository;
    private readonly ILogger<LoanService> _logger;

    public LoanService(IUserRepository userRepository, IBankRepository bankRepository,
        ILoanRepository loanRepository, ILogger<LoanService> logger, IUserAccountService userAccountService, IAccountRepository accountRepository, ITransferService transferService)
    {
        _userRepository = userRepository;
        _bankRepository = bankRepository;
        _loanRepository = loanRepository;
        _logger = logger;
        _userAccountService = userAccountService;
        _accountRepository = accountRepository;
        _transferService = transferService;
    }

    public async Task CreateLoanAsync(LoanDto dto)
    {
        _logger.LogInformation("Starting creation of loan for user {UserId} in bank {BankId} with amount {Amount}, term {Term} months, interestRate {InterestRate}%",
            dto.UserId, dto.BankId, dto.Amount, dto.TermInMonths, dto.InterestRate);

        try
        {
            var bank = await _bankRepository.GetByIdAsync(dto.BankId);
            if (bank == null)
            {
                _logger.LogWarning("Failed to create loan: bank with ID {BankId} not found", dto.BankId);
                throw new ApplicationException($"Bank with id: {dto.BankId} does not exist.");
            }

            var borrower = await _userRepository.GetByIdAsync(dto.UserId);
            if (borrower == null)
            {
                _logger.LogWarning("Failed to create loan: user with ID {UserId} not found", dto.UserId);
                throw new ApplicationException($"User with id: {dto.UserId} does not exist.");
            }
            
            var loan = new Loan(borrower, bank, dto.Amount, dto.TermInMonths, dto.InterestRate, dto.TotalAmount, dto.MonthlyPayment, dto.DestinationAccountId);
        
            await _loanRepository.AddAsync(loan);
            
            _logger.LogInformation("Loan successfully created for user {UserId} in bank {BankId}", dto.UserId, dto.BankId);
        }
        catch (Exception e) when (!(e is ApplicationException))
        {
            _logger.LogError(e, "Error creating loan for user {UserId} in bank {BankId}", dto.UserId, dto.BankId);
            throw;
        }
    }

    public async Task AddLoanAccount(int loanId)
    {
        var loan = await _loanRepository.GetByIdAsync(loanId);
        if (loan == null)
        {
            _logger.LogWarning("Failed to create loan account: loan with ID {LoanId} not found", loanId);
            throw new ApplicationException($"Loan with ID {loanId} does not exist.");
        }
        
        var loanAccountDto = new UserAccountDto
        {
            OwnerId = loan.BorrowerId,
            BankId = loan.BankId,
            Balance = 0,
            AccountType = AccountType.Credit
        };

        var loanAccount = await _userAccountService.CreateUserAccountAsync(loanAccountDto);
        if (loanAccount == null)
        {
            _logger.LogWarning("Failed to create loan account");
            throw new ApplicationException("Loan account could not be created.");
        }
        
        loan.AddLoanAccount(loanAccount);
        await _loanRepository.UpdateAsync(loan);
    }

    public async Task SendLoanAmount(int loanId)
    {
        var loan = await _loanRepository.GetByIdAsync(loanId);
        if (loan == null)
        {
            _logger.LogWarning("Failed to send loan amount: loan with ID {LoanId} not found",
                loanId);
            throw new ApplicationException($"Loan with ID {loanId} does not exist.");
        }

        var transferDto = new TransferDto
        {
            SenderId = loan.LoanAccountId.Value,
            ReceiverId = loan.DestinationAccountId,
            Amount = loan.TotalAmount,
            Type = TransferType.Regular,
            IsSenderCreditAccount = true
        };
        
        await _transferService.CreateTransferAsync(transferDto);
        
        
    }

    public async Task<IEnumerable<Loan>> FetchUserLoansByBankAsync(int userId, int bankId)
    {
        _logger.LogInformation("Fetching loans for user {UserId} in bank {BankId}", userId, bankId);

        try
        {
            var loans = await _loanRepository.GetUserLoansByBankAsync(userId, bankId);
            
            _logger.LogInformation("Successfully fetched loans for user {UserId} in bank {BankId}", userId, bankId);
            
            return loans;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching loans for user {UserId} in bank {BankId}", userId, bankId);
            throw;
        }
    }

    public async Task<IEnumerable<Loan>> FetchLoansByBankAsync(int bankId)
    {
        _logger.LogInformation("Fetching all loans in bank {BankId}", bankId);

        try
        {
            var loans = await _loanRepository.GetLoansByBankAsync(bankId);
            
            _logger.LogInformation("Successfully fetched all loans for bank {BankId}", bankId);

            return loans;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching all loans for bank {BankId}", bankId);
            throw;
        }
    }

    public async Task UpdateStatusAsync(int loanId, RequestStatus newStatus)
    {
        _logger.LogInformation("Updating status of loan {LoanId} to {NewStatus}", loanId, newStatus);

        try
        {
            await _loanRepository.UpdateStatusAsync(loanId, newStatus);
            
            _logger.LogInformation("Successfully updated status of loan {LoanId} to {NewStatus}", loanId, newStatus);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating status of loan {LoanId} to {NewStatus}", loanId, newStatus);
            throw;
        }
    }

    public async Task<bool> RevertLoanCreationAsync(int loanId)
    {
        _logger.LogInformation("Reverting loan creation for loan {LoanID}",
            loanId);
        
        var loan = await _loanRepository.GetByIdAsync(loanId);
        if (loan == null || loan.Status != RequestStatus.Approved)
        {
            _logger.LogWarning("Cannot revert loan creation: loan {LoanId} not found or not approved",
                loanId);
            return false;
        }

        try
        {
            await _loanRepository.UpdateStatusAsync(loanId, RequestStatus.Rejected);

            if (loan.LoanAccountId.HasValue)
            {
                var loanAccount = await _accountRepository.GetByIdAsync(loan.LoanAccountId.Value);
                if (loanAccount != null)
                {
                    var destinationAccount = await _accountRepository.GetByIdAsync(loan.DestinationAccountId);
                    if (destinationAccount != null && destinationAccount.Balance >= loan.TotalAmount)
                    {
                        await _transferService.CreateTransferAsync(new TransferDto
                        {
                            SenderId = loan.DestinationAccountId,
                            ReceiverId = loan.LoanAccountId.Value,
                            Amount = loan.TotalAmount,
                            Type = TransferType.Regular,
                            IsSenderCreditAccount = true
                        });
                    }
                    
                    loanAccount.Deactivate();
                    await _accountRepository.UpdateAsync(loanAccount);
                }
            }
        
            _logger.LogInformation("Successfully reverted loan creation for loan {LoanId}", loanId);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reverting loan creation for loan {LoanId}", loanId);
            return false;
        }
    }

    public async Task<bool> RestoreLoanCreationAsync(int loanId)
    {
        _logger.LogInformation("Restoration loan creation for loan {LoanId}",
            loanId);
        
        var loan = await _loanRepository.GetByIdAsync(loanId);
        if (loan == null)
        {
            _logger.LogWarning("Cannot restore loan creation: loan {LoanId} not found",
                loanId);
            return false;
        }

        try
        {
            await _loanRepository.UpdateStatusAsync(loanId, RequestStatus.Approved);

            if (loan.LoanAccountId.HasValue)
            {
                var loanAccount = await _accountRepository.GetByIdAsync(loan.LoanAccountId.Value);
                if (loanAccount != null)
                {
                    loanAccount.Activate();
                    await _accountRepository.UpdateAsync(loanAccount);

                    await _transferService.CreateTransferAsync(new TransferDto
                    {
                        SenderId = loan.LoanAccountId.Value,
                        ReceiverId = loan.DestinationAccountId,
                        Amount = loan.TotalAmount,
                        Type = TransferType.Regular,
                        IsSenderCreditAccount = true
                    });
                }
            }
        
            _logger.LogInformation("Successfully restored loan creation for loan {LoanId}", loanId);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error restoring loan creation for loan {LoanId}", loanId);
            return false;
        }
    }
}