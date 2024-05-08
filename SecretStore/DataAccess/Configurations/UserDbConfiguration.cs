using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretStore.DataAccess.Models;

namespace SecretStore.DataAccess.Configurations;

public class UserDbConfiguration : IEntityTypeConfiguration<UserDb>
{
    public void Configure(EntityTypeBuilder<UserDb> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.ClientId)
            .IsUnique();
        
        builder.Property(x => x.ClientSecret).IsRequired();

        builder.HasMany(x => x.Groups)
            .WithMany(x => x.Users);

        builder.HasMany(x => x.Tokens)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .IsRequired();
    }
}