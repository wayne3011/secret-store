using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretStore.DataAccess.Models;

namespace SecretStore.DataAccess.Configurations;

public class TokensDbConfiguration : IEntityTypeConfiguration<TokensDb>
{
    public void Configure(EntityTypeBuilder<TokensDb> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.RefreshToken).IsRequired();
        builder.Property(x => x.UserId).IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(x => x.Tokens)
            .HasForeignKey(x => x.UserId)
            .IsRequired();
    }
}