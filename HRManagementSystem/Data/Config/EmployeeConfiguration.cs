using HRManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRManagementSystem.Data.Config
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.Email)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Phone).HasMaxLength(11);

            builder.Property(e => e.Address).HasMaxLength(200);

            builder.Property(e => e.BasicSalary)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.HireDate)
                .HasColumnType("date");

            builder.Property(e => e.UpdatedAt)
                .IsRequired(false);

            builder.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(e => e.DeletedAt)
                .IsRequired(false);

            builder.Property(e => e.IsDeleted)
                .HasDefaultValue(false);


            builder.HasOne(e => e.Position)
                .WithMany(p => p.Employees)
                .HasForeignKey(e => e.PositionId)
                .IsRequired();

            builder.HasMany(e => e.Leaves)
                .WithOne(l => l.Employee)
                .HasForeignKey(l => l.EmployeeId)
                .IsRequired();

            builder.HasMany(e => e.Payrolls)
                .WithOne(p => p.Employee)
                .HasForeignKey(p => p.EmployeeId)
                .IsRequired();

            builder.HasMany(e => e.Attendances)
                .WithOne(a => a.Employee)
                .HasForeignKey(a => a.EmployeeId)
                .IsRequired();

            builder.HasOne(e => e.Manager)
                .WithMany(m => m.Subordinates)
                .HasForeignKey(e => e.ManagerId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Employee");
        }
    }
}
