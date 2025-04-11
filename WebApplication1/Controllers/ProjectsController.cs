using Business.Services;
using Domain.FormModels;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class ProjectsController(ProjectService projectService) : Controller
    {
        private readonly ProjectService _projectService = projectService;

        public IActionResult Projects()
        {
            //if (!User.Identity.IsAuthenticated)
            //    return RedirectToAction("Signup", "SignUp");

            return View();
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
