using HRManagementSystem.Data;
using HRManagementSystem.DTO;
using HRManagementSystem.Enums;
using Microsoft.EntityFrameworkCore;

namespace HRManagementSystem.Services
{
    public class DashboardService
    {
        private readonly AppDBContext _context;

        public DashboardService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<DashboardDto> GetDashboardDataAsync()
        {
            return new DashboardDto
            {
                TotalEmployees = await _context.Employees.CountAsync(),
                DepartmentsCount = await _context.Departments.CountAsync(),
                PositionsCount = await _context.Positions.CountAsync(),
                PendingLeaves = await _context.Leaves.CountAsync(l => l.Status == LeaveStatus.Pending),
                AverageSalary = await _context.Employees.AverageAsync(e => e.BasicSalary),

                LatestEmployees = await _context.Employees
                    .OrderByDescending(e => e.CreatedAt)
                    .Take(5)
                    .Select(e => new LatestEmployeeDto
                    {
                        Id = e.Id,
                        FullName = e.FullName,
                        PositionName = e.Position.Name,
                        HireDate = e.HireDate
                    }).ToListAsync(),

                DepartmentChart = await _context.Departments
                    .Select(d => new DepartmentEmployeeCountDto
                    {
                        DepartmentName = d.Name,
                        EmployeeCount = d.Positions
                            .SelectMany(p => p.Employees)
                            .Count()
                    }).ToListAsync()
            };
        }
    }

}
