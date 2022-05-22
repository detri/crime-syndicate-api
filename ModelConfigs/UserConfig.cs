using CrimeSyndicate.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrimeSyndicate.ModelConfigs;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => u.Name).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.SyndicateId).IsUnique();

        builder.Property(u => u.Id).IsRequired();
        builder.Property(u => u.Name).IsRequired().HasMaxLength(24);
        builder.Property(u => u.Email).IsRequired();
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.SyndicateId).IsRequired(false);

        builder.HasOne(u => u.Syndicate).WithOne(s => s.Owner)
            .HasForeignKey<Syndicate>(s => s.OwnerId);
    }
}