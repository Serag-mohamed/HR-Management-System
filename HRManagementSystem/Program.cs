using HRManagementSystem.Data;
using HRManagementSystem.Interceptors;
using HRManagementSystem.Models;
using HRManagementSystem.Repositories;
using HRManagementSystem.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HRManagementSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("default"))
                .AddInterceptors(new SoftDeleteInterceptor()));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 6;

                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
            })
            .AddEntityFrameworkStores<AppDBContext>()
            .AddDefaultTokenProviders();


            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<EmployeeService>();
            builder.Services.AddScoped<DepartmentService>();
            builder.Services.AddScoped<PayrollService>();
            builder.Services.AddScoped<LeaveService>();
            builder.Services.AddScoped<PositionService>();
            builder.Services.AddScoped<AttendanceService>();
            builder.Services.AddScoped<DashboardService>();

            builder.Services.AddSingleton<IEmailSender, FakeEmailSender>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }

    }
}
