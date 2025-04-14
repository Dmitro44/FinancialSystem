using FinancialSystem.Application.DTOs;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;

namespace FinancialSystem.Application.Interfaces;

public interface IUserService
{
    Task CreateUserAsync(UserDto dto);
    Task<string?> AuthenticateUserAsync(string email, string password);
    Task<User?> GetUserByIdAsync(int userId);
    Task<Role?> GetRoleInBankAsync(int userId, int bankId);
    Task<bool> RevertUserCreation(int userId);
    Task<bool> RestoreUserCreationAsync(int userId);
}