using Business.Services;
using Domain.Dtos;
using Domain.Extensions;
using Domain.FormModels;
using IdentityDatabase.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design.Serialization;
using WebApplication1.ViewModels;


namespace WebApplication1.Controllers
{

    public class DashboardController(ProjectService projectService, ClientService clientService, MemberService memberService, StatusService statusService, UserManager<AppUserEntity> userManager) : Controller
    {

        private readonly StatusService _statusService = statusService;
        private readonly MemberService _memberService = memberService;

        private readonly ProjectService _projectService = projectService;
        private readonly ClientService _clientService = clientService;

        private readonly UserManager<AppUserEntity> _userManager = userManager;

        [Authorize]
        public async Task<IActionResult> Index(string filter = "All")
        {
            // get all data required for the view
            var status = await _statusService.GetAllStatusAsync();
            var members = await _memberService.GetAllMembersAsync();
            var clients = await _clientService.GetAllClientsAsync();
            var dtos = await _projectService.GetAllWithRelationsAsync();


            ViewBag.AllCount = dtos.Count();
            ViewBag.ActiveCount = dtos.Count(p => p.StatusName == "Active");
            ViewBag.CompletedCount = dtos.Count(p => p.StatusName == "Completed");
            ViewBag.OnHoldCount = dtos.Count(p => p.StatusName == "On hold");


            // filter the order by status. 
            var filtered = filter switch
            {
                "Active" => dtos.Where(p => p.StatusName == "Active"),
                "Completed" => dtos.Where(p => p.StatusName == "Completed"),
                "OnHold"    => dtos.Where(p => p.StatusName == "On hold"),
                _ => dtos
            };
            ViewBag.Filter = filter;

            // projectmembers for user when I add the logic for more than 1 member per project
            foreach (var project in filtered)
                project.ProjectMembers = members.Where(m => project.MemberIds.Contains(m.Id)).ToList();

            // send to view
            var model = new ProjectViewModels
            {
                FormModel = new ProjectFormModel(),
                ProjectList = filtered.ToList(),
                Clients = clients,
                Members = members,
                Status = status
            };
            return View(model);
        }
    }
}