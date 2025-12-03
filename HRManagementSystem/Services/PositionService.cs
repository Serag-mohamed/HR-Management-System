using HRManagementSystem.Data;
using HRManagementSystem.DTO;
using Microsoft.EntityFrameworkCore;

namespace HRManagementSystem.Services
{
    public class PositionService
    {
        private readonly AppDBContext _context;

        public PositionService(AppDBContext context)
        {
            this._context = context;
        }

        public async Task<List<PositionDto>> GetAllAsync()
        {
            return await _context.Positions
                .Select(p => new PositionDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync();

        }

        
        public async Task<PositionDetailsDto?> GetPositionWithEmployeesAsync(int id)
        {
            return await _context.Positions
                .Where(p => p.Id == id)
                .Select(p => new PositionDetailsDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CreatedAt = p.CreatedAt,
                    Employees = p.Employees
                        .Select(e => new EmployeeDto
                        {
                            Id = e.Id,
                            Name = e.FullName,
                            HireDate = e.HireDate
                        })
                        .ToList()
                }).FirstOrDefaultAsync();

        }

        public async Task<List<PositionListToAddEmployeeDto>> GetPositionsForAddEmployeeAsync()
        {
            return await _context.Positions
                .Select(p => new PositionListToAddEmployeeDto
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToListAsync();
        }

    }
}

