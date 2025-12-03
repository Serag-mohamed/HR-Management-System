using HRManagementSystem.Data;
using HRManagementSystem.DTO;
using HRManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HRManagementSystem.Services
{
    public class PayrollService
    {
        private readonly AppDBContext _context;

        public PayrollService(AppDBContext context)
        {
            this._context = context;
        }

        public async Task<List<PayrollDetailsDto>> GetPayrollByEmployeeId(int employeeId)
        {
            return await _context.Payrolls
                .Where(p => p.EmployeeId == employeeId)
                .AsNoTracking()
                .OrderByDescending(p => p.PayrollMonth)
                .Select(p => new PayrollDetailsDto
                {
                    BasicSalary = p.BasicSalary,
                    Bonuses = p.Bonuses,
                    Deductions = p.Deductions,
                    NetSalary = p.NetSalary,
                    PayrollMonth = p.PayrollMonth
                })
                .ToListAsync();
        }

        public async Task<List<PayrollManagementGroupsDto>> GetAllPayrolls()
        {
            var payrolls = await _context.Payrolls
                .AsNoTracking()
                .Select(p => new
                {
                    p.Id,
                    EmployeeName = p.Employee.FullName,
                    PositionName = p.Employee.Position.Name,
                    p.BasicSalary,
                    p.Bonuses,
                    p.Deductions,
                    p.NetSalary,
                    p.PayrollMonth
                })
                .ToListAsync();

            var grouped = payrolls
                .GroupBy(p => new { p.PayrollMonth.Year, p.PayrollMonth.Month })
                .Select(g => new PayrollManagementGroupsDto
                {
                    Month = new DateTime(g.Key.Year, g.Key.Month, 1),
                    Payrolls = g.Select(p => new PayrollManagementDto
                    {
                        Id = p.Id,
                        EmployeeName = p.EmployeeName,
                        PositionName = p.PositionName,
                        BasicSalary = p.BasicSalary,
                        Bonuses = p.Bonuses,
                        Deductions = p.Deductions,
                        NetSalary = p.NetSalary
                    }).ToList()
                })
                .OrderByDescending(g => g.Month)
                .ToList();

            return grouped;
        }

        public async Task<List<EmployeeToAddPayrollDto>> GetListOfEmployee()
        {
            return await _context.Employees.AsNoTracking().Select(e => new EmployeeToAddPayrollDto
            {
                Id = e.Id,
                Name = e.FullName,
            }).ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.Where(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> CheckPayrollExists(int employeeId, DateTime payrollMonth)
        {
            Payroll? payroll = await _context.Payrolls.FirstOrDefaultAsync(p => p.EmployeeId == employeeId && p.PayrollMonth == payrollMonth);
            return payroll != null;
        }
    }
}
