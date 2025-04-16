
using Database.Repos;
using Domain.Dtos;
using Domain.Extensions;

namespace Business.Services;

public class StatusService(StatusRepository statusRepository)
{

    private readonly StatusRepository _statusRepository = statusRepository;

    public async Task<IEnumerable<StatusDto>> GetAllStatusAsync()
    {

        var statuses = await _statusRepository.GetAllAsync();
        return statuses.Result.Select(status => status.MapTo<StatusDto>()).ToList();
    }
}
