using HRManagementSystem.Data;
using HRManagementSystem.DTO;
using HRManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HRManagementSystem.Services
{
    public class AttendanceService
    {
        private readonly AppDBContext _context;

        public AttendanceService(AppDBContext context)
        {
            this._context = context;
        }

        public async Task<List<AttendanceDto>> GetMyAttendanceAsync(int employeeId)
        {
            return await _context.Attendances
                .Where(a => a.EmployeeId == employeeId)
                .Select(a => new AttendanceDto
                {
                    Date = a.Date,
                    CheckInTime = a.CheckInTime,
                    CheckOutTime = a.CheckOutTime,
                    TotalHoursWorked = a.TotalHoursWorked
                }).OrderByDescending(a => a.Date).ToListAsync();
        }

        public async Task<Attendance?> IsCheckedInAsync(int employeeId, DateTime date)
        {
            return await _context.Attendances
                .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date.Date == date.Date);
        }

        public async Task<List<AttendanceGroupDto>> GetAttendanceReportAsync()
        {
            var grouped = await _context.Attendances
                .AsNoTracking()
                .GroupBy(a => a.Date)
                .OrderByDescending(g => g.Key)
                .Select(g => new AttendanceGroupDto
                {
                    Date = g.Key,

                    Records = g.Select(a => new AttendanceRecordDto
                    {
                        EmployeeName = _context.Employees
                            .Where(e => e.Id == a.EmployeeId)
                            .Select(e => e.FullName)
                            .FirstOrDefault()!,

                        CheckIn = a.CheckInTime,
                        CheckOut = a.CheckOutTime,
                        TotalHours = a.TotalHoursWorked,

                        Status = a.CheckInTime <= new TimeSpan(9, 0, 0)
                            ? "Present"
                            : "Late"
                    }).ToList()
                })
                .ToListAsync();

            return grouped;
        }

        public async Task<int> GetDepartmentIdByManagerIdAsync(int managerId)
        {
            return await _context.Employees
                .Where(e => e.Id == managerId)
                .Select(e => e.Position.DepartmentId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<AttendanceGroupDto>> GetTeamAttendanceReportAsync(int deptId)
        {
            var grouped = await _context.Attendances
                .AsNoTracking()
                .Where(a => a.Employee.Position.DepartmentId == deptId)
                .OrderByDescending(a => a.Date)
                .GroupBy(a => a.Date)
                .Select(g => new AttendanceGroupDto
                {
                    Date = g.Key,

                    Records = g.Select(a => new AttendanceRecordDto
                    {
                        EmployeeName = a.Employee.FullName,
                        CheckIn = a.CheckInTime,
                        CheckOut = a.CheckOutTime,
                        TotalHours = a.TotalHoursWorked,
                        Status = a.CheckInTime <= new TimeSpan(9, 0, 0)
                            ? "Present"
                            : "Late"
                    }).ToList()
                })
                .ToListAsync();

            return grouped;
        }


    }
}
