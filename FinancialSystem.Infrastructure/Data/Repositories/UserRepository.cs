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
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        var userToUpdate = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
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