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
namespace Business.Services;

public class ProjectService
{
    private readonly AppDbContext _context;

    public ProjectService(AppDbContext context)
    {
        _context = context;
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

        _context.Projects.Add(entity);
        await _context.SaveChangesAsync();
    }


    public async Task<T?> GetAsync<T>(int id)
    {
        var entity = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
        return entity is null ? default : entity.MapTo<T>();
    }
}

