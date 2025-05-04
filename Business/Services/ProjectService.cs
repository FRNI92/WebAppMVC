using Database.Data;
using Database.Entities;
using Domain.Dtos;
using Domain.FormModels;
using IdentityDatabase.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Extensions;
using Database.ReposResult;
using Microsoft.EntityFrameworkCore;
using Database.Repos;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
namespace Business.Services;

public class ProjectService
{
    private readonly ProjectMemberRepository _projectMemberRepository;
    private readonly ProjectRepository _projectRepository;

    public ProjectService(ProjectRepository projectrepository, ProjectMemberRepository projectMemberRepository)
    {
        _projectMemberRepository = projectMemberRepository;
        _projectRepository = projectrepository;
    }

    public async Task CreateAsync(ProjectFormModel form)
    {
        var entity = new ProjectEntity
        {
            Image = form.Image,
            ProjectName = form.ProjectName,
            Description = form.Description,
            StartDate = form.StartDate,
            EndDate = form.EndDate,
            Created = DateTime.Now,
            Budget = form.Budget,
            StatusId = 1, // On hold. user cant set status. only admin in future

            ClientId = form.ClientId,
        };

        var isSuccess = await _projectRepository.AddAsync(entity);
        if (isSuccess.Succeeded)
            await _projectRepository.SaveAsync();

        if (form.MemberIds != null && form.MemberIds.Any())
            foreach (var memberId in form.MemberIds)
            {
                var projectMember = new ProjectMemberEntity
                {
                    ProjectId = entity.Id,
                    MemberId = memberId
                };

                await _projectMemberRepository.AddAsync(projectMember);
            }
            await _projectMemberRepository.SaveAsync();
    }


    public async Task<T?> GetAsync<T>(int id)
    {
        var result = await _projectRepository.GetAsync(p => p.Id == id);
        return result.Result.MapTo<T>();
    }


    public async Task<IEnumerable<T>> GetAllAsync<T>()
    {
        var result = await _projectRepository.GetAllAsync();

        if (!result.Succeeded || result.Result == null)
            return Enumerable.Empty<T>();

        var dtoList = result.Result.Select(entity => entity.MapTo<T>());
        return dtoList;
    }

    public async Task<IEnumerable<ProjectFormDto>> GetAllWithRelationsAsync()
    {
        var entities = await _projectRepository.GetAllWithFullRelationsAsync();

        return entities.Select(e => new ProjectFormDto
        {
            Id = e.Id,
            Image = e.Image,
            ProjectName = e.ProjectName,
            Description = e.Description,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            Created = e.Created,
            TimeLeftText = (e.EndDate.Date - DateTime.Today).Days 
            
            switch
            {
                < 0 => "Past deadline",
                0 => "Ends today",
                < 7 => $"{(e.EndDate.Date - DateTime.Today).Days} days left",
                _ => $"{(e.EndDate.Date - DateTime.Today).Days / 7} weeks left"
            },
            Budget = e.Budget,
            ClientId = e.ClientId,
            ClientName = e.Client?.ClientName,
            StatusId = e.StatusId,
            StatusName = e.Status?.StatusName,
            MemberIds = e.ProjectMembers.Select(pm => pm.MemberId).ToList(),
                    ProjectMembers = e.ProjectMembers
            .Where(pm => pm.Member != null)
            .Select(pm => new MemberDto
            {
                Id = pm.Member.Id,
                Image = pm.Member.Image,
                FirstName = pm.Member.FirstName,
                LastName = pm.Member.LastName
            }).ToList()
        });
    }





    public async Task<ReposResult<ProjectFormDto>> UpdateProjectAsync(ProjectFormDto dto)
    {
        var entity = dto.MapTo<ProjectEntity>();

 
        entity.ProjectMembers = dto.MemberIds.Select(id => new ProjectMemberEntity
        {
            ProjectId = entity.Id,
            MemberId = id
        }).ToList();

        var updatedEntity = await _projectRepository.UpdateAsync(p => p.Id == entity.Id, entity);

        if (updatedEntity != null)
        {
            var result = await _projectRepository.SaveAsync();
            var updatedDto = updatedEntity.MapTo<ProjectFormDto>();

            return new ReposResult<ProjectFormDto>
            {
                Succeeded = true,
                StatusCode = 200,
                Result = updatedDto
            };
        }

        return new ReposResult<ProjectFormDto>
        {
            Succeeded = false,
            StatusCode = 404,
            Error = "Could not update project"
        };
    }



    public async Task<ReposResult<bool>> DeleteProjectAsync(int id)
    {
        await _projectRepository.BeginTransactionAsync();

        try
        {
            var projectExists = await _projectRepository.DoesEntityExistAsync(p => p.Id == id);
            if (!projectExists.Succeeded || !projectExists.Result)
            {
                await _projectRepository.RollBackTransactionAsync();
                return new ReposResult<bool>
                {
                    Succeeded = false,
                    StatusCode = 404,
                    Error = "Project could not be found.",
                    Result = false
                };
            }

            var removeResult = await _projectRepository.RemoveAsync(p => p.Id == id);
            if (!removeResult.Succeeded || !removeResult.Result)
            {
                await _projectRepository.RollBackTransactionAsync();
                return new ReposResult<bool>
                {
                    Succeeded = false,
                    StatusCode = 400,
                    Error = "Failed to remove project.",
                    Result = false
                };
            }

            var saveChanges = await _projectRepository.SaveAsync();
            if (saveChanges.Succeeded && saveChanges.Result > 0)
            {
                await _projectRepository.CommitTransactionAsync();
                return new ReposResult<bool>
                {
                    Succeeded = true,
                    StatusCode = 200,
                    Result = true
                };
            }

            await _projectRepository.RollBackTransactionAsync();
            return new ReposResult<bool>
            {
                Succeeded = false,
                StatusCode = 500,
                Error = "Deletion failed, no changes were saved.",
                Result = false
            };
        }
        catch (Exception ex)
        {
            await _projectRepository.RollBackTransactionAsync();
            Debug.WriteLine($"Could not Delete project {ex.Message} {ex.StackTrace}");
            return new ReposResult<bool>
            {
                Succeeded = false,
                StatusCode = 500,
                Error = "Could not delete project",
                Result = false
            };
        }
    }
}
