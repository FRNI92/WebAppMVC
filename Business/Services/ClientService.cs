using Database.Repos;
using Domain.Dtos;
using Domain.Extensions;

namespace Business.Services;

public class ClientService
{
    private readonly ClientRepository _clientRepository;

    public ClientService(ClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<IEnumerable<ClientDto>> GetAllClientsAsync()
    {
        var clients = await _clientRepository.GetAllAsync();

        // Om MapTo() fungerar korrekt och mappar från ClientEntity till ClientDto:
        return clients.Result.Select(client => client.MapTo<ClientDto>()).ToList();
    }
}
