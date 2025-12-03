using HRManagementSystem.Contract;
using HRManagementSystem.Data.Config;
using HRManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HRManagementSystem.Data
{
    public class AppDBContext : IdentityDbContext<ApplicationUser>
    {
        
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public AppDBContext(DbContextOptions options): base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(EmployeeConfiguration).Assembly);


            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeleteable).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var prop = Expression.Property(parameter, nameof(ISoftDeleteable.IsDeleted));
                    var condition = Expression.Lambda(
                        Expression.Equal(prop, Expression.Constant(false)),
                        parameter
                    );
                    builder.Entity(entityType.ClrType).HasQueryFilter(condition);
                }
            }

        }
    }
}
