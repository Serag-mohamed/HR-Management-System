using HRManagementSystem.Data;
using HRManagementSystem.DTO;
using HRManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HRManagementSystem.Services
{
    public class DepartmentService
    {
        private readonly AppDBContext _context;

        public DepartmentService(AppDBContext context)
        {
            this._context = context;
        }

        public async Task<List<DepartmentDto>> GetAllAsync()
        {
            return await _context.Departments.Select(d => new DepartmentDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt,
                }).ToListAsync();
        }

        public async Task<DepartmentDto?> GetDepartmentWithPositionsAsync(int id)
        {
            var department = await _context.Departments
            .Where(d => d.Id == id)
            .Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt,
                ManagerName = d.Positions
                    .Where(p => p.Name == d.Name + " Manager")
                    .SelectMany(p => p.Employees)
                    .Select(e => e.FullName)
                    .FirstOrDefault() ?? "No Manager Assigned",
                Positions = d.Positions
                    .Select(p => new PositionDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        CreatedAt = p.CreatedAt,
                        EmployeeCount = p.Employees.Count()
                    })
                    .AsEnumerable()
                    .ToList()
            })
            .FirstOrDefaultAsync();


            return department;
        }



    }
}
