using HRManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRManagementSystem.Data.Config
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(d => d.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(d => d.UpdatedAt)
                     .IsRequired(false);

            builder.Property(d => d.DeletedAt)
                   .IsRequired(false);

            builder.Property(d => d.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasMany(d => d.Positions)
                   .WithOne(e => e.Department)
                   .HasForeignKey(e => e.DepartmentId)
                   .IsRequired();

            builder.ToTable("Department");
        }
    }
}
