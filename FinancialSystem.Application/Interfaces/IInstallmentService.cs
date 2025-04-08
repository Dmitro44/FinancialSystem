using FinancialSystem.Application.DTOs;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;

namespace FinancialSystem.Application.Interfaces;

public interface IInstallmentService
{
    Task CreateInstallmentAsync(InstallmentDto dto);
    Task AddInstallmentAccount(int installmentId);
    Task SendInstallmentAmount(int installmentId);
    Task<IEnumerable<Installment>> FetchUserInstallmentsByBankAsync(int userId, int bankId);
    Task<IEnumerable<Installment>> FetchInstallmentsByBankAsync(int bankId);
    Task UpdateStatusAsync(int installmentId, RequestStatus newStatus);
}