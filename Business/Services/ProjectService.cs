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
namespace Business.Services;

public class ProjectService
{

    private readonly ProjectRepository _projectRespository;

    public ProjectService(ProjectRepository projectrepository)
    {
        _projectRespository = projectrepository;
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
            ClientId = 1, // Tillfälligt (byt när du kopplar clientval)
        };

        var isSuccess = await _projectRespository.AddAsync(entity);
        if (isSuccess.Succeeded)
            await _projectRespository.SaveAsync();
    }


    public async Task<T?> GetAsync<T>(int id)
    {
        var result = await _projectRespository.GetAsync(p => p.Id == id);
        return result.Result.MapTo<T>();
    }
}

