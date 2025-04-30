
using Microsoft.EntityFrameworkCore;
using Database.Data;
using Database.Entities;
using IdentityDatabase.Data;
using Database.Repos;
using Database.ReposResult;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Business.Services;

public class NotificationService(NoificationRepository notificationRepository, IdentityAppContext identityContext)
{
    private readonly NoificationRepository _notificationRepository = notificationRepository;
    private readonly IdentityAppContext _identityContext = identityContext;

    public async Task<ReposResult<bool>> AddNotificationAsync(NotificationEntity entity)
    {
        if (string.IsNullOrEmpty(entity.Icon))
        {
            entity.Icon = entity.NotificationTypeId switch
            {
                1 => "/images/Logout.svg",
                2 => "/images/company_logo.svg",
                _ => entity.Icon
            };
        }

        var addResult = await _notificationRepository.AddAsync(entity);
        if (!addResult.Succeeded)
            return new ReposResult<bool> { Succeeded = false, StatusCode = addResult.StatusCode, Error = addResult.Error };

        var saveResult = await _notificationRepository.SaveAsync();
        return new ReposResult<bool>
        {
            Succeeded = saveResult.Succeeded,
            StatusCode = saveResult.StatusCode,
            Error = saveResult.Error,
            Result = saveResult.Result > 0
        };
    }

    public async Task<IEnumerable<NotificationEntity>> GetNotificationsAsync(int memberId, int take = 10)
    {
        var dismissedResult = await _notificationRepository.GetDismissedIdsAsync(memberId);
        var dismissedIds = dismissedResult.Succeeded ? dismissedResult.Result! : new List<string>();

        return await _notificationRepository.GetRecentUndismissedAsync(dismissedIds, take);
    }

    public async Task<int> GetMemberIdFromUserIdAsync(string userId)
    {
        return await _identityContext.Users
            .Where(x => x.Id == userId)
            .Select(x => x.MemberId ?? 0)
            .FirstOrDefaultAsync();
    }

    public async Task<ReposResult<bool>> DismissNotificationAsync(string notificationId, int memberId)
    {
        return await _notificationRepository.DismissNotificationAsync(notificationId, memberId);
    }
}





