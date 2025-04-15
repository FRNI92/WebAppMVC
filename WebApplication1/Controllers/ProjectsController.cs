using Business.Services;
using Domain.Dtos;
using Domain.Extensions;
using Domain.FormModels;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class ProjectsController(ProjectService projectService, ClientService clientService) : Controller
    {
        private readonly ProjectService _projectService = projectService;
        private readonly ClientService _clientService = clientService;
        public async Task<IActionResult> Projects()
        {
            var clients = await _clientService.GetAllClientsAsync();

            var dtos = await _projectService.GetAllWithRelationsAsync();

            var model = new ProjectViewModels
            {
                FormModel = new ProjectFormModel(),
                ProjectList = dtos, // detta är redan IEnumerable<ProjectFormDto>
                Clients = clients
            };

            return View(model);
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
            return RedirectToAction("Projects");
        }
    }
}
