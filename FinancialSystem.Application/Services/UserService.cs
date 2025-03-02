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
        var sw = Stopwatch.StartNew();
        var existingUser = await _userRepository.IsUserExists(dto.IdentificationNumber);
        sw.Stop();
        Console.WriteLine($"IsUserExists: {sw.ElapsedMilliseconds}");

        if (existingUser) throw new InvalidOperationException("User already exists");
        
        var user = new User(dto.FirstName, dto.LastName, dto.Patronymic,
            dto.PassportNumber, dto.PassportSeries, dto.IdentificationNumber,
            dto.PhoneNumber, dto.Email);
        
        sw.Restart();
        user.SetPassword(dto.Password);
        sw.Stop();
        Console.WriteLine($"SetPassword: {sw.ElapsedMilliseconds}");

        sw.Restart();
        await _userRepository.AddAsync(user);
        sw.Stop();
        Console.WriteLine($"Saving user took {sw.ElapsedMilliseconds}");
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
}