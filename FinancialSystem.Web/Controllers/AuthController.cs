using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Services;
using FinancialSystem.Domain.Interfaces;
using FinancialSystem.Infrastructure.Data;
using FinancialSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialSystem.Web.Controllers;

public class AuthController : Controller
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

            var sw = Stopwatch.StartNew();
            await _userService.CreateUserAsync(userDto);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        
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
        if (ModelState.IsValid)
        {
            var token = await _userService.AuthenticateUserAsync(model.Email, model.Password);

            if (token == null)
            {
                ModelState.AddModelError("", "Invalid login attempt");
            }
            return Ok(token);
            // return RedirectToAction("Index", "Home");
        }
        
        return View(model);
    }
}