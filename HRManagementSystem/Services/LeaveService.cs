using HRManagementSystem.Data;
using HRManagementSystem.DTO;
using Microsoft.EntityFrameworkCore;

namespace HRManagementSystem.Services
{
    public class LeaveService
    {

        private readonly AppDBContext _context;

        public LeaveService(AppDBContext context)
        {
            this._context = context;
        }

        public async Task<bool> IsExistedLeave(DateTime start, DateTime end)
        {
            var leave = await _context.Leaves.FirstOrDefaultAsync(l =>
                l.StartDate <= end &&
                l.EndDate >= start
            );

            return leave != null;
        }


        public async Task<List<LeaveDto>> GetLeavesForEmployeeAsync(int empId)
        {
            var leaves = await _context.Leaves
                .Where(a => a.EmployeeId == empId)  
                .OrderByDescending(a => a.StartDate)        
                .Select(a => new LeaveDto
                {
                    Id = a.Id,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    LeaveType = a.LeaveType.ToString(),
                    DaysNamer = a.TotalDays,
                    RequestDate = a.RequestDate,
                    Status = a.Status.ToString()
                })
                .ToListAsync();

            return leaves;
        }


        public async Task<List<LeaveReportDto>> GetAllLeavesAsync()
        {
            var leaves = await _context.Leaves
                .AsNoTracking()
                .OrderByDescending(a => a.StartDate)
                .Select(a => new LeaveReportDto
                {
                    EmployeeName = a.Employee.FullName,
                    PositionName = a.Employee.Position.Name,
                    LeaveType = a.LeaveType.ToString(),
                    RequestDate = a.RequestDate,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    DaysNamer = a.TotalDays,
                    Status = a.Status.ToString()
                })
                .ToListAsync();

            return leaves;
        }
        public async Task<int> GetDepartmentIdByManagerIdAsync(int managerId)
        {
            return await _context.Employees
                .Where(e => e.Id == managerId)
                .Select(e => e.Position.DepartmentId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<LeaveReportDto>> GetTeamLeavesByDeptIdAsync(int deptId)
        {
            return await _context.Leaves
                .AsNoTracking()
                .Where(l => l.Employee.Position.DepartmentId == deptId)
                .OrderByDescending(l => l.StartDate)
                .Select(l => new LeaveReportDto
                {
                    Id = l.Id,
                    EmployeeName = l.Employee.FullName,
                    PositionName = l.Employee.Position.Name,
                    LeaveType = l.LeaveType.ToString(),
                    RequestDate = l.RequestDate,
                    StartDate = l.StartDate,
                    EndDate = l.EndDate,
                    DaysNamer = l.TotalDays,
                    Status = l.Status.ToString()
                })
                .ToListAsync();
        }

    }
}

