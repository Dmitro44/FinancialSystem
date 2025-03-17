using System.Diagnostics;
using FinancialSystem.Application.DTOs;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Interfaces;

namespace FinancialSystem.Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtService _jwtService;

    public UserService(IUserRepository userRepository, JwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task CreateUserAsync(UserDto dto)
    {
        var existingUser = await _userRepository.IsUserExists(dto.IdentificationNumber);

        if (existingUser) throw new InvalidOperationException("User already exists");
        
        var user = new User(dto.FirstName, dto.LastName, dto.Patronymic,
            dto.PassportNumber, dto.PassportSeries, dto.IdentificationNumber,
            dto.PhoneNumber, dto.Email);
        
        user.SetPassword(dto.Password);

        await _userRepository.AddAsync(user);
    }

    public async Task<string?> AuthenticateUserAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        
        if (user == null || !user.VerifyPassword(password))
        {
            return null;
        }

        return _jwtService.GenerateToken(user);
    }

    public async Task<User?> GetUserAsync(int userId)
    {
        return await _userRepository.GetByIdAsync(userId);
    }

    public async Task<Role?> GetRoleInBankAsync(int userId, int bankId)
    {
        var role = await _userRepository.GetRoleInBankAsync(userId, bankId);

        if (role == null)
        {
            throw new InvalidOperationException("User has no role in this bank");
        }

        return role;
    }
}