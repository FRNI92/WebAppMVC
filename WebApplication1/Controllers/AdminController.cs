using Business.Services;
using IdentityDatabase.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    public class AdminController(UserManager<AppUserEntity> userManager, MemberService memberService) : Controller
    {
        private readonly UserManager<AppUserEntity> _userManager = userManager;
        private readonly MemberService _memberService = memberService;

        public IActionResult AdminLogin()
        {
            return View();
        }

    }
}
