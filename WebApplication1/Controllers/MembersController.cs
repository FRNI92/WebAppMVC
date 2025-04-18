using Business.Services;
using Domain.FormModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class MembersController(MemberService memberService, IWebHostEnvironment env) : Controller
    {
        public IActionResult Members()
        {

            var model = new MemberViewModel
            {

                FormModel = new MemberFormModel(),
            
            };
            return View(model);
        }

    public async Task<IActionResult> Add(MemberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    errors = ModelState.ToDictionary(
                        x => x.Key,
                        x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        )
                });
            }
            if (model.FormModel.ImageFile != null && model.FormModel.ImageFile.Length > 0)
            {
                var uploadFolder = Path.Combine(env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadFolder); // Skapa om den inte finns

                var originalName = Path.GetFileName(model.FormModel.ImageFile.FileName);
                var fileName = $"{Guid.NewGuid()}_{originalName}";
                var filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.FormModel.ImageFile.CopyToAsync(stream);
                }

                model.FormModel.Image = fileName;
            }

            await memberService.CreateMemberAsync(model.FormModel);
            return RedirectToAction("Members", "Members");
        }
    }
}

//    [Authorize(Roles = "Administrator")]
//    [HttpPost]
//    public async Task<IActionResult> Add(ProjectViewModels model)
//    {
//        if (!ModelState.IsValid)
//        {
//            return BadRequest(new
//            {
//                errors = ModelState.ToDictionary(
//                    x => x.Key,
//                    x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray()
//                )
//            });
//        }
//        if (model.FormModel.ImageFile != null && model.FormModel.ImageFile.Length > 0)
//        {
//            var uploadFolder = Path.Combine(_env.WebRootPath, "uploads");
//            Directory.CreateDirectory(uploadFolder); // Skapa om den inte finns

//            var originalName = Path.GetFileName(model.FormModel.ImageFile.FileName);
//            var fileName = $"{Guid.NewGuid()}_{originalName}";
//            var filePath = Path.Combine(uploadFolder, fileName);

//            using (var stream = new FileStream(filePath, FileMode.Create))
//            {
//                await model.FormModel.ImageFile.CopyToAsync(stream);
//            }

//            model.FormModel.Image = fileName;
//        }

//        await _projectService.CreateAsync(model.FormModel);
//        return RedirectToAction("Index", "Dashboard");
//    }