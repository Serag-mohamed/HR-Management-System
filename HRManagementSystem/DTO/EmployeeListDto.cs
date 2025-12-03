using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HRManagementSystem.DTO
{
    public class EmployeeListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PositionName { get; set; }
        public DateOnly HireDate { get; set; }
    }
}
