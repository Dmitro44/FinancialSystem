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
}