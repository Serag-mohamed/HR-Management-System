using HRManagementSystem.Contract;

namespace HRManagementSystem.Models
{
    public class Position : ISoftDeleteable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int DepartmentId { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Department Department { get; set; }
        public ICollection<Employee> Employees { get; set; } = [];

        public void Delete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }

        public void UndoDelete()
        {
            IsDeleted = false;
            DeletedAt = null;
        }
    }
}
