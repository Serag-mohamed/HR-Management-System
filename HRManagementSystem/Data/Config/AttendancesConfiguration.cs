using HRManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRManagementSystem.Data.Config
{
    public class AttendancesConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.EmployeeId)
                .IsRequired();

            builder.Property(a => a.Date)
                .IsRequired()
                .HasDefaultValueSql("CONVERT(date, GETUTCDATE())");

            builder.Property(a => a.CheckInTime)
                .IsRequired();

            builder.Property(a => a.CheckOutTime)
                .IsRequired(false);

            builder.Property(a => a.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(a => a.UpdatedAt)
                .IsRequired(false);

            builder.Property(a => a.DeletedAt)
                .IsRequired(false);

            builder.Property(a => a.IsDeleted)
                .HasDefaultValue(false);

            builder.HasIndex(a => new { a.EmployeeId, a.Date })
                   .IsUnique();

            builder.ToTable("Attendance");

        }
    }
}
