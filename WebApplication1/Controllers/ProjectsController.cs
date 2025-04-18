using Business.Services;
using Domain.Dtos;
using Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class ProjectsController(ProjectService projectService,IWebHostEnvironment env, ClientService clientService, MemberService memberService, StatusService statusService): Controller
    {

        private readonly IWebHostEnvironment _env = env;


        private readonly StatusService _statusService = statusService;

        private readonly MemberService _memberService = memberService;


        private readonly ProjectService _projectService = projectService;
        private readonly ClientService _clientService = clientService;
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Add(ProjectViewModels model)
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
                var uploadFolder = Path.Combine(_env.WebRootPath, "uploads");
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

            await _projectService.CreateAsync(model.FormModel);
            return RedirectToAction("Index", "Dashboard");
        }

        //authorize want you to be logged in. roles admin want you to be admin to access
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Update(ProjectViewModels form)
        {
            if (form.FormModel.ImageFile != null && form.FormModel.ImageFile.Length > 0)
            {
                var uploadFolder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadFolder);

                var originalName = Path.GetFileName(form.FormModel.ImageFile.FileName);
                var fileName = $"{Guid.NewGuid()}_{originalName}";
                var filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await form.FormModel.ImageFile.CopyToAsync(stream);
                }
                form.FormModel.Image = fileName;
            }

            var dto = form.FormModel.MapTo<ProjectFormDto>();
            var result = await _projectService.UpdateProjectAsync(dto);
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _projectService.DeleteProjectAsync(id);

            if (!result.Succeeded)
            {
                // Visa ett felmeddelande på samma vy eller annan vy
                TempData["Error"] = result.Error;
                return RedirectToAction("Index", "Dashboard");
            }

            return RedirectToAction("Index", "Dashboard");
        }
    }
}

