
namespace HRManagementSystem.DTO
{
    public class AttendanceRecordDto
    {
        public string EmployeeName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan CheckIn { get; set; }
        public TimeSpan? CheckOut { get; set; }
        public float TotalHours { get; set; }
        public string Status { get; set; }
    }
}
