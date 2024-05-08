using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using SecretStore.DataAccess.Models;

namespace SecretStore.DataAccess.Context;
public class MyDbContext : DbContext
{
    public DbSet<UserDb> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<SecretDb> Credentials { get; set; }
    public DbSet<TokensDb> Tokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
}