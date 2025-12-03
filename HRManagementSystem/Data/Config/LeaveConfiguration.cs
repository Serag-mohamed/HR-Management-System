using HRManagementSystem.Enums;
using HRManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRManagementSystem.Data.Config
{
    public class LeaveConfiguration : IEntityTypeConfiguration<Leave>
    {
        public void Configure(EntityTypeBuilder<Leave> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.LeaveType)
               .HasConversion<string>()
               .IsRequired()
               .HasMaxLength(50);

            builder.Property(l => l.RequestDate)
                .IsRequired();

            builder.Property(l => l.StartDate)
                .IsRequired();

            builder.Property(l => l.EndDate)
                .IsRequired();

            builder.Property(l => l.Status)
               .HasConversion<string>()
               .IsRequired()
               .HasMaxLength(50)
               .HasDefaultValue(LeaveStatus.Pending);

            builder.Property(l => l.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(l => l.ApprovalBy)
                .WithMany(u => u.ApprovedLeaves)
                .HasForeignKey(l => l.ApprovalById)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(l => l.UpdatedAt)
                .IsRequired(false);


            builder.ToTable("Leave");
        }
    }
}
