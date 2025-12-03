using HRManagementSystem.Contract;

namespace HRManagementSystem.Models
{
    public class Employee : ISoftDeleteable
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public decimal BasicSalary { get; set; }
        public DateOnly HireDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public int PositionId { get; set; }
        public int? ManagerId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Position Position { get; set; }
        public ICollection<Leave> Leaves { get; set; } = [];
        public ICollection<Payroll> Payrolls { get; set; } = [];
        public ICollection<Attendance> Attendances { get; set; } = [];
        public Employee? Manager { get; set; }
        public ICollection<Employee> Subordinates { get; set; } = [];

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
