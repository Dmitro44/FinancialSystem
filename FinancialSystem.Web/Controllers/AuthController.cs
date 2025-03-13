using System.Diagnostics;
using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Services;
using FinancialSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSystem.Web.Controllers;

public class AuthController : BaseController
{
    private readonly UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        try
        {
            var userDto = new UserDto
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IdentificationNumber = model.IdentificationNumber,
                PassportNumber = model.PassportNumber,
                PassportSeries = model.PassportSeries,
                Patronymic = model.Patronymic,
                PhoneNumber = model.PhoneNumber,
                Password = model.Password
            };

            await _userService.CreateUserAsync(userDto);
        
            return RedirectToAction("Login", "Auth");
        }
        catch (InvalidOperationException e)
        {
            ModelState.AddModelError("", e.Message);
            return View(model);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }
        
        if (ModelState.IsValid)
        {
            
            var token = await _userService.AuthenticateUserAsync(model.Email, model.Password);

            if (token == null)
            {
                ModelState.AddModelError("","Invalid login or password");
                return View(model);
            }
            
            Response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });
        
            return RedirectToAction("GetBanks", "Bank");
            
        }
        
        return View(model);
    }

    public IActionResult Logout()
    {
        Response.Cookies.Delete("AuthToken");
        return RedirectToAction("Login", "Auth");
    }
}