using Business.Services;
using Domain.Dtos;
using Domain.Extensions;
using Domain.FormModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design.Serialization;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class DashboardController(ProjectService projectService, ClientService clientService, MemberService memberService, StatusService statusService) : Controller
    {

        private readonly StatusService _statusService = statusService;
        private readonly MemberService _memberService = memberService;

        private readonly ProjectService _projectService = projectService;
        private readonly ClientService _clientService = clientService;
        public async Task<IActionResult> Index()
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
    }
}
