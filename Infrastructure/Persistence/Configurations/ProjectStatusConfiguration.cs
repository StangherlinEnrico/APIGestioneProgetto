using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ProjectStatusConfiguration : IEntityTypeConfiguration<ProjectStatus>
{
    public void Configure(EntityTypeBuilder<ProjectStatus> builder)
    {
        builder.ToTable("ProjectStatuses");

        builder.HasKey(ps => ps.Id);

        builder.Property(ps => ps.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(ps => ps.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(ps => ps.Name)
            .IsUnique();
    }
}
