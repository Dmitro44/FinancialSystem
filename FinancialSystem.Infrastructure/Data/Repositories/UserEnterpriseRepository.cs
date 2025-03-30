using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialSystem.Infrastructure.Data.Repositories;

public class UserEnterpriseRepository : IUserEnterpriseRepository
{
    private readonly ApplicationDbContext _context;

    public UserEnterpriseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserEnterprise?> GetByIdAsync(int id)
    {
        return await _context.UserEnterprises
            .Include(ue => ue.User)
            .Include(ue => ue.Enterprise)
            .FirstOrDefaultAsync(ue => ue.Id == id);
    }

    public async Task AddAsync(UserEnterprise userEnterprise)
    {
        await _context.UserEnterprises.AddAsync(userEnterprise);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserEnterprise userEnterprise)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(int userEnterpriseId)
    {
        var userEnterpriseToDelete = await _context.UserEnterprises.FindAsync(userEnterpriseId);
        if (userEnterpriseToDelete == null)
        {
            throw new ArgumentNullException(nameof(userEnterpriseToDelete));
        }
        
        _context.UserEnterprises.Remove(userEnterpriseToDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<UserEnterprise?> GetByUserAndEnterpriseId(int userId, int enterpriseId)
    {
        return await _context.UserEnterprises
            .Include(ue => ue.User)
            .Include(ue => ue.Enterprise)
            .FirstOrDefaultAsync(ue => ue.UserId == userId && ue.EnterpriseId == enterpriseId);
    }

    public async Task<IEnumerable<UserEnterprise>> GetByUserIdAsync(int userId)
    {
        return await _context.UserEnterprises
            .Include(ue => ue.Enterprise)
            .Where(ue => ue.UserId == userId)
            .ToListAsync();
    }
}