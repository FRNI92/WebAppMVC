﻿using Database.Data;
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
namespace Business.Services;

public class ProjectService
{

    private readonly ProjectRepository _projectRepository;

    public ProjectService(ProjectRepository projectrepository)
    {
        _projectRepository = projectrepository;
    }

    public async Task CreateAsync(ProjectFormModel form)
    {
        var entity = new ProjectEntity
        {
            Image = form.Image,
            ProjectName = form.ProjectName,
            Description = form.Description,
            StartDate = form.StartDate ?? DateTime.Now,
            EndDate = form.EndDate ?? DateTime.Now.AddDays(7),
            Created = DateTime.Now,
            Budget = form.Budget ?? 0,
            StatusId = 1, // On hold

            ClientId = form.ClientId, // Tillfälligt (byt när du kopplar clientval)
            MemberId = form.MemberId
        };

        var isSuccess = await _projectRepository.AddAsync(entity);
        if (isSuccess.Succeeded)
            await _projectRepository.SaveAsync();
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
        var includes = new Expression<Func<ProjectEntity, object>>[]
        {
        project => project.Client,
        project => project.Status,
        project => project.Members
        };

        var result = await _projectRepository.GetAllAsync(includes: includes);

        if (!result.Succeeded || result.Result == null)
            return Enumerable.Empty<ProjectFormDto>();

        return result.Result.Select(e => new ProjectFormDto
        {
            Id = e.Id,
            Image = e.Image,
            ProjectName = e.ProjectName,
            Description = e.Description,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            Created = e.Created,

            TimeLeftText = (e.EndDate.Date - DateTime.Today).Days switch
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
            MemberId = e.MemberId,
            MemberNames = e.Members.Select(m => $"{m.FirstName} {m.LastName}").ToList()
        });
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
