using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Interfaces;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;
using FinancialSystem.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialSystem.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, IJwtService jwtService, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _logger = logger;
    }

    public async Task CreateUserAsync(UserDto dto)
    {
        _logger.LogInformation("Starting user creation for {Email} with identification number {IdentificationNumber}", 
            dto.Email, dto.IdentificationNumber);

        try
        {
            var existingUser = await _userRepository.IsUserExists(dto.IdentificationNumber);

            if (existingUser)
            {
                _logger.LogWarning("Failed to create user: user with identification number {IdentificationNumber} already exists", 
                    dto.IdentificationNumber);
                throw new InvalidOperationException("User already exists");
            }
        
            var user = new User(dto.FirstName, dto.LastName, dto.Patronymic,
                dto.PassportNumber, dto.PassportSeries, dto.IdentificationNumber,
                dto.PhoneNumber, dto.Email);
        
            user.SetPassword(dto.Password);

            await _userRepository.AddAsync(user);
        
            _logger.LogInformation("User successfully created with ID {UserId} and email {Email}", 
                user.Id, user.Email);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating user with email {Email}", dto.Email);
            throw;
        }
    }

    public async Task<string?> AuthenticateUserAsync(string email, string password)
    {
        _logger.LogInformation("Authenticating user with email {Email}", email);

        try
        {
            var user = await _userRepository.GetByEmailAsync(email);
        
            if (user == null)
            {
                _logger.LogWarning("Authentication failed: user with email {Email} not found", email);
                return null;
            }

            if (!user.VerifyPassword(password))
            {
                _logger.LogWarning("Authentication failed: invalid password for user {Email}", email);
                return null;
            }

            var token = _jwtService.GenerateToken(user);
        
            _logger.LogInformation("User {Email} successfully authenticated", email);

            return token;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error during authentication for user {Email}", email);
            throw;
        }
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        _logger.LogInformation("Retrieving user with ID {UserId}", userId);

        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            
            _logger.LogInformation("Successfully retrieved user with ID {UserId}", userId);

            return user;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error retrieving user with ID {UserId}", userId);
            throw;
        }
    }

    public async Task<Role?> GetRoleInBankAsync(int userId, int bankId)
    {
        _logger.LogInformation("Retrieving role for user {UserId} in bank {BankId}",
            userId, bankId);

        try
        {
            var role = await _userRepository.GetRoleInBankAsync(userId, bankId);

            if (role == null)
            {
                _logger.LogWarning("User {UserId} has no role in bank {BankId}", 
                    userId, bankId);
                throw new InvalidOperationException("User has no role in this bank");
            }
            
            _logger.LogInformation("Successfully retrieved role for user {UserId} in bank {BankId}", 
                userId, bankId);

            return role;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error retrieving role for user {UserId} in bank {BankId}",
                userId, bankId);
            throw;
        }
    }
}