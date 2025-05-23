﻿using Database.Entities;
using Database.Repos;
using Database.ReposResult;
using Domain.Dtos;
using Domain.Extensions;
using Domain.FormModels;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Business.Services;

public class MemberService(MemberRepository memberRepository)
{
    private readonly MemberRepository _memberRepository = memberRepository;

    public async Task<MemberDto?> GetByIdAsync(int id)// how we get the currently logged in member
    {
        var result = await _memberRepository.GetAsync(m => m.Id == id);

        if (!result.Succeeded || result.Result == null)
            return null;

        return result.Result.MapTo<MemberDto>();
    }

    public async Task<ReposResult<bool>> CreateMemberAsync(MemberDto dto)
    {
       

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






    public async Task<ReposResult<bool>> UpdateMemberAsync(MemberDto dto)
    {
        var entity = dto.MapTo<MemberEntity>();

        var updateResult = await _memberRepository.UpdateAsync(m => m.Id == dto.Id, entity);
        if (!updateResult.Succeeded)
            return new ReposResult<bool>
            {
                Succeeded = false,
                StatusCode = updateResult.StatusCode,
                Error = updateResult.Error
            };

        var saveResult = await _memberRepository.SaveAsync();
        return new ReposResult<bool>
        {
            Succeeded = saveResult.Succeeded,
            StatusCode = saveResult.StatusCode,
            Result = saveResult.Result > 0
        };
    }










    public async Task<ReposResult<bool>> DeleteMemberAsync(int id)
    {
        await _memberRepository.BeginTransactionAsync();

        try
        {
            var memberExists = await _memberRepository.DoesEntityExistAsync(p => p.Id == id);
            if (!memberExists.Succeeded || !memberExists.Result)
            {
                await _memberRepository.RollBackTransactionAsync();
                return new ReposResult<bool>
                {
                    Succeeded = false,
                    StatusCode = 404,
                    Error = "Member could not be found.",
                    Result = false
                };
            }

            var removeResult = await _memberRepository.RemoveAsync(p => p.Id == id);
            if (!removeResult.Succeeded || !removeResult.Result)
            {
                await _memberRepository.RollBackTransactionAsync();
                return new ReposResult<bool>
                {
                    Succeeded = false,
                    StatusCode = 400,
                    Error = "Failed to remove Member.",
                    Result = false
                };
            }

            var saveChanges = await _memberRepository.SaveAsync();
            if (saveChanges.Succeeded && saveChanges.Result > 0)
            {
                await _memberRepository.CommitTransactionAsync();
                return new ReposResult<bool>
                {
                    Succeeded = true,
                    StatusCode = 200,
                    Result = true
                };
            }

            await _memberRepository.RollBackTransactionAsync();
            return new ReposResult<bool>
            {
                Succeeded = false,
                StatusCode = 500,
                Error = "Deletion failed, no changes were savd.",
                Result = false
            };
        }
        catch (Exception ex)
        {
            await _memberRepository.RollBackTransactionAsync();
            Debug.WriteLine($"Could not Delete member {ex.Message} {ex.StackTrace}");
            return new ReposResult<bool>
            {
                Succeeded = false,
                StatusCode = 500,
                Error = "Could not delete member",
                Result = false
            };
        }
    }
}
