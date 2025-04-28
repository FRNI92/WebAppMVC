using Business.Services;
using Database.Entities;
using Domain.Dtos;
using Domain.FormModels;
using Domain.FormModels.SignUpFormModel;
using IdentityDatabase.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging.Abstractions;
using System.Security.Claims;
using WebApplication1.Hubs;

namespace WebApplication1.Controllers;

public class AccountController(UserService userService, SignInManager<AppUserEntity> signInManager, UserManager<AppUserEntity> userManager, NotificationService notificationService, IHubContext<NotificationHub> notificationHub, MemberService memberService) : Controller
{

    private readonly UserService _userService = userService;

    //use the entitity directly. has to do with identity
    private readonly SignInManager<AppUserEntity> _signInManager = signInManager;

    private readonly UserManager<AppUserEntity> _userManager = userManager;

    private readonly MemberService _memberService = memberService;
    private readonly NotificationService _notificationService = notificationService;
    private readonly IHubContext<NotificationHub> _notificationHub = notificationHub;
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

        var user = await _userService.GetUserByEmailAsync(model.Email);
        if (user == null)
        {
            ViewBag.LoginError = "User not found.";
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            user, model.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            if (user.MemberId != null)
            {
                var member = await _memberService.GetByIdAsync(user.MemberId.Value);

                var notificationEntity = new NotificationEntity
                {
                    Message = $"{member.FirstName} {member.LastName} signed in.",
                    NotificationTypeId = 1,
                    TargetGroupId = 1
                };

                await _notificationService.AddNotificationAsync(notificationEntity);
                var notifications = await _notificationService.GetNotificationsAsync(user.MemberId.Value);
                var newNotification = notifications.OrderByDescending(x => x.Created).FirstOrDefault();

                if (newNotification != null)
                {
                    await _notificationHub.Clients.All.SendAsync("ReceiveNotification", newNotification);
                }
            }

            return RedirectToAction("Index", "Dashboard");
        }

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

        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            ModelState.AddModelError("Email", "That email is already registered.");
            return View(model);
        }

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
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);// so I cant be mistaken for logged in not member - guest
        await _signInManager.SignOutAsync();
        return RedirectToAction("SignIn", "Account");
    }



    [HttpPost]
    public IActionResult ExternalSignInWithGoogle(string provider, string returnUrl = null!)
    {
        if(string.IsNullOrEmpty(provider))
        {
            ModelState.AddModelError("", "Invalid provider");
            return View("SignIn");
        }

        var redirectUrl = Url.Action("ExternalSignInWithGoogleCallBack", "Account", new { returnUrl })!;
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }


    //rename and reuse this if I have time to add more 3rd login
    public async Task <IActionResult> ExternalSignInWithGoogleCallBack(string returnUrl = null!, string remoteError = null!)
    {
        returnUrl ??= Url.Content("~/");
        if(!string.IsNullOrEmpty(remoteError))
        {
            ModelState.AddModelError("", $"Error from external proider: {remoteError}");
            return View("SignIn");
        }
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
            return RedirectToAction("SignIn");

        var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        if (signInResult.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {

            // If I change my entity and add more fields I can implement this 
            // add firstname and lastname in the new appuserentity in that case
            //string firstName = string.Empty;
            //string lastName = string.Empty;

            //try
            //{
            //    firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "";
            //    lastName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? "";
            //}
            //catch {  }




            string email = info.Principal.FindFirstValue(ClaimTypes.Email)!;
            string username = $"ext_{info.LoginProvider.ToLower()}_{email}";

            var user = new AppUserEntity { UserName = username, Email = email };

            var indentityResult = await _userManager.CreateAsync(user);
            if (indentityResult.Succeeded)
            {
                await _userManager.AddLoginAsync(user, info);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }
            foreach(var error in indentityResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View("SignIn");

        }
    }
}



