using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretStore.DataAccess.Models;

namespace SecretStore.DataAccess.Configurations;

public class GroupDbConfiguration : IEntityTypeConfiguration<GroupDb>
{
    public void Configure(EntityTypeBuilder<GroupDb> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired();
        
        builder.HasIndex(e => e.Name)
            .IsUnique();

        builder.HasMany(x => x.Users)
            .WithMany(x => x.Groups);

        builder.HasMany(x => x.Secrets)
            .WithOne(x => x.Group)
            .HasForeignKey(x => x.GroupId)
            .IsRequired();
    }
}