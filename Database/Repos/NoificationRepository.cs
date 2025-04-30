

using Database.Data;
using Database.Entities;
using Database.ReposResult;
using Microsoft.EntityFrameworkCore;

namespace Database.Repos;

public class NoificationRepository(AppDbContext context) : BaseRepository<NotificationEntity>(context)
{
    private readonly AppDbContext _context = context;

    public async Task<List<NotificationEntity>> GetRecentUndismissedAsync(List<string> dismissedIds, int take = 10)
    {
        return await _context.Notifications
            .Where(x => !dismissedIds.Contains(x.Id))
            .OrderByDescending(x => x.Created)
            .Take(take)
            .ToListAsync();
    }

    public async Task<ReposResult<List<string>>> GetDismissedIdsAsync(int memberId)
    {
        try
        {
            var ids = await _context.DismissedNotifications
                .Where(x => x.MemberId == memberId)
                .Select(x => x.NotificationId)
                .ToListAsync();

            return new ReposResult<List<string>>
            {
                Succeeded = true,
                StatusCode = 200,
                Result = ids
            };
        }
        catch (Exception ex)
        {
            return new ReposResult<List<string>>
            {
                Succeeded = false,
                StatusCode = 500,
                Error = $"Error in GetDismissedIdsAsync: {ex.Message}"
            };
        }
    }

    public async Task<ReposResult<bool>> DismissNotificationAsync(string notificationId, int memberId)
    {
        try
        {
            var alreadyDismissed = await _context.DismissedNotifications
                .AnyAsync(x => x.NotificationId == notificationId && x.MemberId == memberId);

            if (alreadyDismissed)
            {
                return new ReposResult<bool>
                {
                    Succeeded = true,
                    StatusCode = 200,
                    Result = false
                };
            }

            await _context.DismissedNotifications.AddAsync(new NotificationDismissedEntity
            {
                MemberId = memberId,
                NotificationId = notificationId
            });

            var saved = await _context.SaveChangesAsync();

            return new ReposResult<bool>
            {
                Succeeded = true,
                StatusCode = 200,
                Result = saved > 0
            };
        }
        catch (Exception ex)
        {
            return new ReposResult<bool>
            {
                Succeeded = false,
                StatusCode = 500,
                Error = $"Error in DismissNotificationAsync: {ex.Message}"
            };
        }
    }
}
