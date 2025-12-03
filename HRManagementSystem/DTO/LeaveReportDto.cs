using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.DTO
{
    public class LeaveReportDto
    {
        public int Id { get; set; }

        [Display(Name = "Employee")]
        public string EmployeeName { get; set; }

        [Display(Name = "Position")]
        public string PositionName { get; set; }

        [Display(Name = "Leave Type")]
        public string LeaveType { get; set; }

        [Display(Name = "Request Date")]
        public DateOnly RequestDate { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Number of Days")]
        public int DaysNamer { get; set; }
        public string Status { get; set; }
    }
}
