using System.Diagnostics;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialSystem.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    public async Task<User?> GetByEmailAsync(string userEmail)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
    }

    public async Task<bool> IsUserExists(string userIdentificationNumber)
    {
        var sw = Stopwatch.StartNew();
        var result = await _context.Users.AsNoTracking().AnyAsync(x => x.IdentificationNumber == userIdentificationNumber);
        sw.Stop();
        Console.WriteLine($"EF Core took: {sw.ElapsedMilliseconds} ms");
        return result;
    }

    public async Task AddAsync(User user)
    {
        if (await _context.Users.FindAsync(user.Id) != null)
        {
            throw new ArgumentException("User already exists", nameof(user));
        }
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        var userToUpdate = await _context.Users.FindAsync(user.Id);
        if (userToUpdate == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        
        _context.Users.Update(userToUpdate);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int userId)
    {
        var userToDelete = await _context.Users.FindAsync(userId);
        if (userToDelete == null)
        {
            throw new ArgumentNullException(nameof(userToDelete));
        }
        
        _context.Users.Remove(userToDelete);
        await _context.SaveChangesAsync();
    }
}