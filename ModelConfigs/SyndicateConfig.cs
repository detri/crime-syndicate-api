using CrimeSyndicate.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrimeSyndicate.ModelConfigs;

public class SyndicateConfig : IEntityTypeConfiguration<Syndicate>
{
    public void Configure(EntityTypeBuilder<Syndicate> builder)
    {
        builder.HasIndex(s => s.Name).IsUnique();
        builder.HasIndex(s => s.OwnerId).IsUnique();

        builder.Property(s => s.Id).IsRequired();
        builder.Property(s => s.Description).IsRequired(false).IsUnicode();
        builder.Property(s => s.Name).IsRequired().HasMaxLength(24);
        builder.Property(s => s.Picture).IsRequired(false);
        builder.Property(s => s.OwnerId).IsRequired();

        builder.HasOne(s => s.Owner).WithOne(u => u.Syndicate);
    }
}