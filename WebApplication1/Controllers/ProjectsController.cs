using Business.Services;
using Domain.Dtos;
using Domain.Extensions;
using Domain.FormModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design.Serialization;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class ProjectsController(ProjectService projectService, ClientService clientService, IWebHostEnvironment env, MemberService memberService, StatusService statusService) : Controller
    {


        private readonly StatusService _statusService = statusService;

        private readonly MemberService _memberService = memberService;



        private readonly IWebHostEnvironment _env = env;

        private readonly ProjectService _projectService = projectService;
        private readonly ClientService _clientService = clientService;
        public async Task<IActionResult> Projects()
        {
            var status = await _statusService.GetAllStatusAsync();
            var members = await _memberService.GetAllMembersAsync();
            var clients = await _clientService.GetAllClientsAsync();

            var dtos = await _projectService.GetAllWithRelationsAsync();

            var model = new ProjectViewModels
            {

                FormModel = new ProjectFormModel(),
                ProjectList = dtos, // detta är redan IEnumerable<ProjectFormDto>
                Clients = clients,
                Members = members,
                Status = status
            };

            return View(model);
        }

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
            return RedirectToAction("Projects");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _projectService.DeleteProjectAsync(id);

            if (!result.Succeeded)
            {
                // Visa ett felmeddelande på samma vy eller annan vy
                TempData["Error"] = result.Error;
                return RedirectToAction("Projects");
            }

            return RedirectToAction("Projects");
        }
    }
}
