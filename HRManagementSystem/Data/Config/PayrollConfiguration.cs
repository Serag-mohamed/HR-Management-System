using HRManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRManagementSystem.Data.Config
{
    public class PayrollConfiguration : IEntityTypeConfiguration<Payroll>
    {
        public void Configure(EntityTypeBuilder<Payroll> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.BasicSalary)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Bonuses)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Deductions)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.NetSalary)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.PayrollMonth)
                .IsRequired();

            builder.Property(p => p.GeneratedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(p => p.CreatedBy)
               .WithMany(u => u.CreatedPayrolls)
               .HasForeignKey(p => p.CreatedById)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.SetNull);

            builder.ToTable("Payroll");

        }
    }
}