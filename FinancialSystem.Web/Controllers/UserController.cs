using FinancialSystem.Application.Services;
using FinancialSystem.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSystem.Web.Controllers;

[Authorize]
public class UserController : BaseController
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var user = await _userService.GetUserAsync(GetCurrentUserId());

        if (user == null)
        {
            return NotFound();
        }
        
        var model = new ProfileViewModel
        {
            LastName = user.LastName,
            FirstName = user.FirstName,
            Patronymic = user.Patronymic,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };

        return View(model);
    }
}