
namespace HRManagementSystem.DTO
{
    public class AttendanceDto
    {
        public DateTime Date { get; set; }
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan? CheckOutTime { get; set; }
        public float TotalHoursWorked { get; set; }
    }
}
