using Database.Entities;
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


    public async Task<bool> AddClientAsync(ClientDto dto)
    {
        var entity = dto.MapTo<ClientEntity>();
        var result = await _clientRepository.AddAsync(entity);
        if (result.Succeeded)
            await _clientRepository.SaveAsync();

        return result.Succeeded;
    }

    public async Task<bool> UpdateClientAsync(ClientDto dto)
    {
        var entity = dto.MapTo<ClientEntity>();
        var result = await _clientRepository.UpdateAsync(x => x.Id == dto.Id, entity);
        if (result.Succeeded)
            await _clientRepository.SaveAsync();

        return result.Succeeded;
    }

    public async Task<bool> DeleteClientAsync(int id)
    {
        var result = await _clientRepository.RemoveAsync(x => x.Id == id);
        if (result.Succeeded)
            await _clientRepository.SaveAsync();

        return result.Succeeded;
    }
}
