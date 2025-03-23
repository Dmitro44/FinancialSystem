using FinancialSystem.Domain.Entities;

namespace FinancialSystem.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}