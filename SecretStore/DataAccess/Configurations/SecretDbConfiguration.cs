using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretStore.DataAccess.Models;

namespace SecretStore.DataAccess.Configurations;

public class SecretDbConfiguration : IEntityTypeConfiguration<SecretDb>
{
    public void Configure(EntityTypeBuilder<SecretDb> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Occupied).IsRequired();
        builder.Property(x => x.GroupId).IsRequired();

        builder.HasOne(x => x.Group)
            .WithMany(x => x.Secrets)
            .HasForeignKey(x => x.GroupId)
            .IsRequired();
    }
}