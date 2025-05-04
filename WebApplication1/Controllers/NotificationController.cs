using Business.Services;
using Database.Entities;
using IdentityDatabase.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using WebApplication1.Hubs;

namespace WebApplication1.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class NotificationController(UserManager<AppUserEntity> userManager, IHubContext<NotificationHub> notificationHub, NotificationService notificationService) : ControllerBase
{
    private readonly IHubContext<NotificationHub> _notificationHub = notificationHub;
    private readonly NotificationService _notificationService = notificationService;
    private readonly UserManager<AppUserEntity> _userManager = userManager;

    [Authorize(Roles = "Administrator")]
    [HttpPost]// not in use. project and member goes straight to signalR
    public async Task<IActionResult> CreateNotification(NotificationEntity notificationEntity)
    {
        await _notificationService.AddNotificationAsync(notificationEntity);
        var notifications = await _notificationService.GetNotificationsAsync( 1, 10);
        var newNotification = notifications.OrderByDescending(x => x.Created).FirstOrDefault();

        if (newNotification != null)
        {
            await _notificationHub.Clients.All.SendAsync("ReceiveNotification", newNotification);
        }

        return Ok(new { success = true });
    }
    [Authorize(Roles = "Administrator")]
    [HttpGet]
    public async Task<IActionResult> GetNotifications()// not in use,project and member goes straight to signalR
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);
        if (user?.MemberId == null)
            return NotFound();

        var notifications = await _notificationService.GetNotificationsAsync(user.MemberId.Value);
        return Ok(notifications);
    }

    [HttpPost("dismiss/{id}")]// this is the only API I user
    public async Task<IActionResult> DismissNotification(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || user.MemberId == null)
            return NotFound();

        await _notificationService.DismissNotificationAsync(id, user.MemberId.Value);
        await _notificationHub.Clients.User(userId).SendAsync("NotificationDismissed", id);
        return Ok(new { success = true });
    }
}
