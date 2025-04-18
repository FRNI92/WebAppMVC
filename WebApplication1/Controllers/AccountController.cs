﻿using Business.Services;
using Domain.Dtos;
using Domain.FormModels;
using Domain.FormModels.SignUpFormModel;
using IdentityDatabase.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

public class AccountController(UserService userService, SignInManager<AppUserEntity> signInManager) : Controller
{

    private readonly UserService _userService = userService;

          //use the entitity directly. has to do with identity
    private readonly SignInManager<AppUserEntity> _signInManager = signInManager;


    public IActionResult SignIn()
    {

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(SignInFormModel model)
    {

        if (!ModelState.IsValid)
        {
            ViewBag.LoginError = "Please fill in all required fields correctly.";
            return View(model);
        }
        var result = await _signInManager.PasswordSignInAsync(
            model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
            return RedirectToAction("Index", "Dashboard");
        ViewBag.LoginError = "Invalid login attempt.";
        return View(model);
    }

    public IActionResult SignUp()
    {
        var model = new SignUpFormModel();
        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpFormModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var dto = new SignUpDto
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            PassMord = model.Password
        };

        var success = await _userService.CreateAsync(dto);
        if (success)
            return RedirectToAction("SignIn", "Account");

        ModelState.AddModelError("NotCreated", "User could not be created.");
        return View(model);
    }


    public new async Task<IActionResult> SignOut()
    {

        await _signInManager.SignOutAsync();
        return RedirectToAction("SignIn", "Account");
    }
    
}



