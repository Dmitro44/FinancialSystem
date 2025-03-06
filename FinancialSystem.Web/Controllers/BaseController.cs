using Microsoft.AspNetCore.Mvc;

namespace FinancialSystem.Web.Controllers;

public class BaseController : Controller
{
    public int GetCurrentUserId()
    {
        var userIdClaims = User.Claims.FirstOrDefault(x => x.Type == "UserId");
        if (userIdClaims == null)
        {
            throw new InvalidOperationException("UserId not found in JWT Token");
        }

        if (!int.TryParse(userIdClaims.Value, out var userId))
        {
            throw new InvalidOperationException("Invalid UserId format in JWT Token");
        }
        
        return userId;
    }

    public string GetUserFullName()
    {
        var firstName = User.Claims.FirstOrDefault(x => x.Type == "FirstName")?.Value ?? "Unknown";
        var lastName = User.Claims.FirstOrDefault(x => x.Type == "LastName")?.Value ?? "User";

        return $"{lastName} {firstName}";
    }
}