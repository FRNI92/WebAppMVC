using Business.Services;
using Domain.Dtos;
using Domain.Extensions;
using Domain.FormModels;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class ProjectsController(ProjectService projectService) : Controller
    {
        private readonly ProjectService _projectService = projectService;

        public async Task<IActionResult> Projects()
        {
            var dto = await _projectService.GetAsync<ProjectFormDto>(6);
                    var model = dto.MapTo<ProjectFormModel>(); // om du har en sån metod

            return View(model); // skickar rätt typ till vyn
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProjectFormModel model)
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

            await _projectService.CreateAsync(model);
            return Ok(); // Eller return RedirectToAction("Projects");
        }
    }
}
