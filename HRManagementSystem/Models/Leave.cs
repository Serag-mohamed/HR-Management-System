using HRManagementSystem.Enums;

namespace HRManagementSystem.Models
{
    public class Leave
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public LeaveType LeaveType { get; set; }
        public DateOnly RequestDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDays => (EndDate.Date - StartDate.Date).Days + 1;
        public LeaveStatus Status { get; set; }
        public string? ApprovalById { get; set; }
        public ApplicationUser? ApprovalBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Employee Employee { get; set; }


    }
}

