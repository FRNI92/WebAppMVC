using Business.Services;
using Database.Entities;
using Domain.Dtos;
using Domain.Extensions;
using IdentityDatabase.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApplication1.Hubs;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class ProjectsController(ProjectService projectService, IWebHostEnvironment env, ClientService clientService, MemberService memberService, StatusService statusService, NotificationService notificationService, IHubContext<NotificationHub> notificationHub, UserManager<AppUserEntity> userManager) : Controller
    {

        private readonly IWebHostEnvironment _env = env;

        private readonly StatusService _statusService = statusService;
        private readonly MemberService _memberService = memberService;
        private readonly ProjectService _projectService = projectService;
        private readonly ClientService _clientService = clientService;

        private readonly NotificationService _notificationService = notificationService;
        private readonly IHubContext<NotificationHub> _notificationHub = notificationHub;

        private readonly UserManager<AppUserEntity> _userManager = userManager;

        [Authorize]// so that members and admin can create project
        [HttpPost]
        public async Task<IActionResult> Add(ProjectViewModels model)
        {
            var appUser = await _userManager.GetUserAsync(User);

            if (appUser?.MemberId == null)
            {
                return RedirectToAction("AdminLogin", "AdminController");
            }

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

            var notification = new NotificationEntity
            {
                Message = $"New project {model.FormModel.ProjectName} was created",
                NotificationTypeId = 2,
                TargetGroupId = 1, // AllUsers!
                Icon = "/uploads/" + model.FormModel.Image
            };

            await _notificationService.AddNotificationAsync(notification);

            await _notificationHub.Clients.All.SendAsync("ReceiveNotification", new
            {
                id = notification.Id,
                message = notification.Message,
                icon = "/uploads/" + model.FormModel.Image,
                created = notification.Created
            });


            return RedirectToAction("Index", "Dashboard");
        }

        //authorize want you to be logged in. roles admin want you to be admin to access
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Update(ProjectViewModels form)
        {
            if (!ModelState.IsValid)
            {
                // Fyll på dropdown-listor igen
                form.Status = await _statusService.GetAllStatusAsync();
                form.Clients = await _clientService.GetAllClientsAsync();
                form.Members = await _memberService.GetAllMembersAsync();
                // unsure if I should return form or not
                return RedirectToAction("Index", "Dashboard");
            }

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


        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _projectService.DeleteProjectAsync(id);

            if (!result.Succeeded)
            {
                // show error message
                TempData["Error"] = result.Error;
                return RedirectToAction("Index", "Dashboard");
            }

            return RedirectToAction("Index", "Dashboard");
        }
    }
}

