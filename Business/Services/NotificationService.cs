
using Microsoft.EntityFrameworkCore;
using Database.Data;
using Database.Entities;
using IdentityDatabase.Data;

namespace Business.Services;

public class NotificationService(AppDbContext context, IdentityAppContext identityContext)
{
    private readonly AppDbContext _context = context;
    private readonly IdentityAppContext _identityContext = identityContext;

    public async Task AddNotificationAsync(NotificationEntity notificationEntity)
    {
        if (string.IsNullOrEmpty(notificationEntity.Icon))
        {
            switch (notificationEntity.NotificationTypeId)
            {
                case 1:
                    notificationEntity.Icon = "/images/Logout.svg";
                    break;
                case 2:
                    notificationEntity.Icon = "/images/company_logo.svg";
                    break;
            }
        }

        _context.Add(notificationEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<NotificationEntity>> GetNotificationsAsync(int memberId, int take = 10)
    {
        var dismissedIds = await _context.DismissedNotifications
            .Where(x => x.MemberId == memberId)
            .Select(x => x.NotificationId)
            .ToListAsync();

        var notifications = await _context.Notifications
            .Where(x => !dismissedIds.Contains(x.Id))
            .OrderByDescending(x => x.Created)
            .Take(take)
            .ToListAsync();

        return notifications;
    }

    public async Task<int> GetMemberIdFromUserIdAsync(string userId)
    {
        return await _identityContext.Users
            .Where(x => x.Id == userId)
            .Select(x => x.MemberId ?? 0)
            .FirstOrDefaultAsync();
    }

    public async Task DismissNotificationAsync(string notificationId, int memberId)
    {
        var alreadyDismissed = await _context.DismissedNotifications
            .AnyAsync(x => x.NotificationId == notificationId && x.MemberId == memberId);

        if (!alreadyDismissed)
        {
            var dismissed = new NotificationDismissedEntity
            {
                NotificationId = notificationId,
                MemberId = memberId
            };

            _context.Add(dismissed);
            await _context.SaveChangesAsync();
        }
    }
}
