using Business.Services;
using IdentityDatabase.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [Authorize]// if you click on a the save button that I left for everyone to see. you will be redirected here
    public class AdminController() : Controller
    {
        public IActionResult AdminLogin()
        {
            return View();
        }

    }
}
