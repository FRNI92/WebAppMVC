
using Database.Entities;
using Database.Repos;
using Domain.Dtos;
using Domain.Extensions;

namespace Business.Services;

public class AddressService(AddressRepository addressRepository)
{
    private readonly AddressRepository _addressRepository = addressRepository;

    public async Task<AddressDto?> GetByIdAsync(int id)
    {
        var result = await _addressRepository.GetAsync(x => x.Id == id);
        return result.Succeeded && result.Result != null
            ? result.Result.MapTo<AddressDto>()
            : null;
    }
    //return clients.Result.Select(client => client.MapTo<ClientDto>()).ToList();
    public async Task<int> CreateAsync(AddressDto dto)
    {
        var entity = dto.MapTo<AddressEntity>();
        await _addressRepository.AddAsync(entity);
        await _addressRepository.SaveAsync();
        return entity.Id;
    }
}
