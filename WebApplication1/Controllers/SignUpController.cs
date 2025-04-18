//using Business.Services;
//using Domain.Dtos;
//using Domain.FormModels;
//using Domain.FormModels.SignUpFormModel;
//using IdentityDatabase.Entities;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//namespace WebApplication1.Controllers
//{
//    public class SignUpController(UserService userService, SignInManager<AppUserEntity> signInManager) : Controller
//    {
//        private readonly UserService _userService = userService;


//        private readonly SignInManager<AppUserEntity> _signInManager = signInManager;

//        //sign in part
//        public IActionResult SignIn()
//        {
//            //var model = new SignInFormModel();
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> SignIn(SignInFormModel model)
//        {

//            if (!ModelState.IsValid)
//            {
//                ViewBag.LoginError = "Please fill in all required fields correctly.";
//                return View(model);
//            }
//            var result = await _signInManager.PasswordSignInAsync(
//                model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

//            if (result.Succeeded)
//                return RedirectToAction("Projects", "Projects");
//            ViewBag.LoginError = "Invalid login attempt.";
//            return View(model);
//        }



//    }
//}
