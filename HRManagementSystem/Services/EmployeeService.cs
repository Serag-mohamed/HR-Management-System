using HRManagementSystem.Data;
using HRManagementSystem.DTO;
using HRManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HRManagementSystem.Services
{
    public class EmployeeService
    {

        private readonly AppDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeeService(AppDBContext context, UserManager<ApplicationUser> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }


        public async Task<List<EmployeeListDto>> GetAllAsync()
        {
            return await _context.Employees
                .Select(e => new EmployeeListDto
                {
                    Id = e.Id,
                    Name = e.FullName,
                    Email = e.Email,
                    HireDate = e.HireDate,
                    PositionName = e.Position.Name
                }).ToListAsync();
             
        }

        public async Task<List<PositionListToAddEmployeeDto>> GetPositionToAddEmployeeAsync()
        {
            return await _context.Positions.Select(p => new PositionListToAddEmployeeDto
            {
                Id = p.Id,
                Name = p.Name
            }).ToListAsync();
        }

        public async Task<List<ManagerListToAddEmployeeDto>> GetManagerListDtoToAddEmployeesAsync()
        {
            var managers = await _context.Employees
                .Where(e => e.Position.Name.EndsWith("Manager"))
                .Select(e => new ManagerListToAddEmployeeDto
                {
                    Id = e.Id,
                    Name = e.FullName
                })
                .ToListAsync();

            return managers;
        }

        public async Task<Employee?> GetByEmailWithIgnorFilterAsync(string email)
        {
            return await _context.Employees.IgnoreQueryFilters().Where(e => e.Email ==  email).FirstOrDefaultAsync();
        }

        public async Task<EmployeeDetailsDto?> GetEmployeeDetaitsAsync(int id)
        {
            return await _context.Employees
                .Where(e => e.Id == id)
                .Select(e => new EmployeeDetailsDto
                {
                    Id = e.Id,
                    Name = e.FullName,
                    Email = e.Email,
                    Phone = e.Phone,
                    Address = e.Address,
                    HireDate = e.HireDate,
                    BasicSalary = e.BasicSalary,
                    CreatedAt = e.CreatedAt,

                    PositionName = e.Position.Name,
                    PositionDescription = e.Position.Description,

                    DepartmentName = e.Position.Department.Name,

                    ManagerName = _context.Positions
                    .Where(p =>
                        p.DepartmentId == e.Position.DepartmentId &&
                        p.Name == (e.Position.Department.Name == "Information Technology (IT)" ? "IT Manager" : e.Position.Department.Name + " Manager")
                    )
                    .SelectMany(p => p.Employees)
                    .Select(emp => emp.FullName)
                    .FirstOrDefault() ?? "No Manager Assigned"
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> BlockUserAccountAsync(int employeeId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.EmployeeId == employeeId);

            if (user == null)
                return false;

            user.LockoutEnabled = true;
            user.LockoutEnd = DateTime.MaxValue;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<IdentityResult> CreateEmployeeUserAsync(ApplicationUser user, string password, string role)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return result; 

            await _userManager.AddToRoleAsync(user, role);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateEmployeeUserAsync(int id, string userName, string displayName)
        {
            var existingUser = await _userManager.FindByIdAsync(id.ToString());
            if (existingUser == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = $"User with Id {id} not found."
                });
            }

            existingUser.Email = userName;
            existingUser.UserName = userName;
            existingUser.DisplayName = displayName;

            var result = await _userManager.UpdateAsync(existingUser);
            return result;
        }

        public async Task<string> GetRoleAsync(int id)
        {
            var positionName = await _context.Positions.Where(p => p.Id == id)
                .Select(p => p.Name).FirstOrDefaultAsync();
            if (positionName == "HR Manager")
                return "HR";
            else if (positionName.EndsWith("Manager", StringComparison.OrdinalIgnoreCase))
                return "Manager";
            else
                return "Employee";
        }

        public async Task<List<EmployeeOfTeamDto>> GetTeamEmployeesByDepartmentIdAsync(int id)
        {
            return await _context.Departments.AsNoTracking().Where(d => d.Id == id)
                .SelectMany(d => d.Positions)
                .SelectMany(p => p.Employees)
                .Select(e => new EmployeeOfTeamDto
                {
                    Id = e.Id,
                    Name = e.FullName,
                    Email = e.Email,
                    PhoneNumber = e.Phone,
                    PositionName = e.Position.Name,
                    HireDate = e.HireDate
                }).ToListAsync();
        }

        public async Task<int> GetDepartmentIdByManagerIdAsync(int managerId)
        {
            return await _context.Employees
                .Where(e => e.Id == managerId)
                .Select(e => e.Position.DepartmentId)
                .FirstOrDefaultAsync();
        }

        public async Task<string?> GetTeamNameByDepartmentIdAsync(int deptId)
        {
            return await _context.Departments
                .Where(d => d.Id == deptId)
                .Select(d => d.Name)
                .FirstOrDefaultAsync();
        }
    }

}

