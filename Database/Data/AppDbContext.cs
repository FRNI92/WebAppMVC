﻿
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)

{
    public DbSet<ProjectEntity> Projects { get; set; }
    public DbSet<ClientEntity> Clients { get; set; }
    public DbSet<MemberEntity> Members { get; set; }
    public DbSet<AddressEntity> Addresses { get; set; }
    public DbSet<StatusEntity> Statuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<StatusEntity>().HasData(
            new StatusEntity { Id = 1, StatusName = "On hold" },
            new StatusEntity { Id = 2, StatusName = "Active" },
            new StatusEntity { Id = 3, StatusName = "Completed" }
        );
        modelBuilder.Entity<ClientEntity>().HasData(
        new ClientEntity { Id = 1, ClientName = "IKEA" },
        new ClientEntity { Id = 2, ClientName = "GitLab Inc." }
    );

        modelBuilder.Entity<MemberEntity>().HasData(
            new MemberEntity
            {
                Id = 1,
                FirstName = "Fredrik",
                LastName = "Nilsson",
                JobTitle = "Developer",
                Email = "fredrik@domain.com",
                Phone = "070-123 45 67",
                AvatarUrl = "/images/fredrik.png",
                DateOfBirth = new DateTime(1990, 1, 1),
                AddressId = 1
            },
            new MemberEntity
            {
                Id = 2,
                FirstName = "Elin",
                LastName = "Andersson",
                JobTitle = "Designer",
                Email = "elin@domain.com",
                Phone = "070-987 65 43",
                AvatarUrl = "/images/elin.png",
                DateOfBirth = new DateTime(1992, 5, 10),
                AddressId = 2
            }
        );

        modelBuilder.Entity<AddressEntity>().HasData(
            new AddressEntity { Id = 1, StreetName = "Testgatan", StreetNumber = "1", PostalCode = "12345", City = "Stockholm" },
            new AddressEntity { Id = 2, StreetName = "Exempelvägen", StreetNumber = "2", PostalCode = "54321", City = "Göteborg" }
        );
    }
}