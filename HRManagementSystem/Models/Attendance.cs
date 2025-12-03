using HRManagementSystem.Contract;

namespace HRManagementSystem.Models
{
    public class Attendance : ISoftDeleteable
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan? CheckOutTime { get; set; }
        public float TotalHoursWorked =>
        CheckOutTime.HasValue ? (float)(CheckOutTime.Value - CheckInTime).TotalHours : 0;
        public DateTime CreatedAt { get; set; }
        public Employee Employee { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }

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
