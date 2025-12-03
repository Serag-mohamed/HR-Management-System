namespace HRManagementSystem.DTO
{
    public class LeaveDto
    {
        public int Id { get; set; }
        public string LeaveType { get; set; }
        public DateOnly RequestDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DaysNamer { get; set; }
        public string Status { get; set; }
    }
}
