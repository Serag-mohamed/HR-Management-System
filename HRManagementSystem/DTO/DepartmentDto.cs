using HRManagementSystem.Models;

namespace HRManagementSystem.DTO
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string ManagerName { get; set; }
        public List<PositionDto> Positions { get; set; } = new List<PositionDto>();
    }
}
