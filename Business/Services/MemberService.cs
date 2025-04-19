using Database.Entities;
using Database.Repos;
using Database.ReposResult;
using Domain.Dtos;
using Domain.Extensions;
using Domain.FormModels;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public class MemberService(MemberRepository memberRepository, AddressRepository addressRepository)
{
    private readonly MemberRepository _memberRepository = memberRepository;
    private readonly AddressRepository _addressRepository = addressRepository;


    public async Task<ReposResult<bool>> CreateMemberAsync(MemberDto dto, AddressDto addressDto)
    {
        //addresses
        var addressEntity = new AddressEntity
        {

            City = addressDto.City,
            StreetName = addressDto.StreetName,
            StreetNumber = addressDto.StreetNumber,
            PostalCode = addressDto.PostalCode
        };
        await _addressRepository.AddAsync(addressEntity);
        await _addressRepository.SaveAsync();
        //addresses
        dto.AddressId = addressEntity.Id;
        var entity = dto.MapTo<MemberEntity>();

        var addResult = await _memberRepository.AddAsync(entity);
        if (!addResult.Succeeded)
            return new ReposResult<bool>
            {
                Succeeded = false,
                StatusCode = 400,
                Error = addResult.Error
            };

        var saveResult = await _memberRepository.SaveAsync();
        if (!saveResult.Succeeded)
            return new ReposResult<bool>
            {
                Succeeded = false,
                StatusCode = 500,
                Error = saveResult.Error
            };

        return new ReposResult<bool>
        {
            Succeeded = true,
            StatusCode = 200,
            Result = true
        };
    }
    public async Task<IEnumerable<MemberDto>> GetAllMembersAsync()
    {
        var members = await _memberRepository.GetAllAsync();

        // Om MapTo() fungerar korrekt och mappar från ClientEntity till ClientDto:
        return members.Result.Select(member => member.MapTo<MemberDto>()).ToList();
    }
}
