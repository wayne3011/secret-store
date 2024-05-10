using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using SecretStore.DataAccess.Models;

namespace SecretStore.DataAccess.Context;
public class StoreDbContext : DbContext
{
    public StoreDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    public DbSet<UserDb> Users { get; set; }
    public DbSet<GroupDb> Groups { get; set; }
    public DbSet<SecretDb> Secrets { get; set; }
    public DbSet<TokensDb> Tokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
}