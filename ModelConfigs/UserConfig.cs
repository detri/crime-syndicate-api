using CrimeSyndicate.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrimeSyndicate.ModelConfigs;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasAlternateKey(u => u.Name);
        builder.HasAlternateKey(u => u.Email);
        
        builder.Property(u => u.Id).IsRequired();
        builder.Property(u => u.Name).IsRequired().HasMaxLength(24);
        builder.Property(u => u.Email).IsRequired();
        builder.Property(u => u.PasswordHash).IsRequired();
    }
}