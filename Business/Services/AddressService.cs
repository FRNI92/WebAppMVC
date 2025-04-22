
using Database.Entities;
using Database.Repos;
using Database.ReposResult;
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

    public async Task<ReposResult<AddressDto>> UpdateAsync(AddressDto dto)
    {
        var result = await _addressRepository.GetAsync(a => a.Id == dto.Id);

        if (result == null || result.Succeeded == false)
        {
            return new ReposResult<AddressDto>
            {
                Succeeded = false,
                Error = "Address not found"
            };
        }

        var entity = result.Result; // address entity

        entity.StreetName = dto.StreetName;
        entity.StreetNumber = dto.StreetNumber;
        entity.PostalCode = dto.PostalCode;
        entity.City = dto.City;

        var saveResult = await _addressRepository.SaveAsync();
        if (saveResult.Succeeded)
        {
            return new ReposResult<AddressDto>
            {
                Succeeded = true,
                Result = entity.MapTo<AddressDto>()
            };
        }

        return new ReposResult<AddressDto>
        {
            Succeeded = false,
            Error = "Failed to save address."
        };
    }
}
