using HRManagementSystem.Models;
using HRManagementSystem.Repositories;
using HRManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HRManagementSystem.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IRepository<Attendance> _repository;
        private readonly AttendanceService _attendanceService;

        public AttendanceController(IRepository<Attendance> repository ,AttendanceService attendanceService)
        {
            this._repository = repository;
            this._attendanceService = attendanceService;
        }

        [HttpGet]
        [Authorize(Roles = "HR,Admin")]
        public async Task<IActionResult> Index()
        {
            var report = await _attendanceService.GetAttendanceReportAsync();
            return View(report);
        }

        [HttpGet]
        [Authorize(Roles = "Employee,Manager,HR")]
        public async Task<IActionResult> MyAttendance()
        {
            var claimValue = User.FindFirst("EmployeeId")?.Value;
            int currentEmployeeId = 0;
            if (!string.IsNullOrEmpty(claimValue))
            {
                int.TryParse(claimValue, out currentEmployeeId);
            }

            var myAttendances = await _attendanceService.GetMyAttendanceAsync(currentEmployeeId);
            return View(myAttendances);

        }

        [HttpGet]
        [Authorize(Roles = "Employee,Manager,HR")]
        public async Task<IActionResult> CheckIn()
        {
            var claimValue = User.FindFirst("EmployeeId")?.Value;
            int currentEmployeeId = 0;
            if (!string.IsNullOrEmpty(claimValue))
            {
                int.TryParse(claimValue, out currentEmployeeId);
            }

            var isCheckedIn = await _attendanceService.IsCheckedInAsync(currentEmployeeId, DateTime.UtcNow);
            if(isCheckedIn != null)
            {
                TempData["Message"] = "You have recorded attendess before";
                return RedirectToAction("MyAttendance");
            }
                
            Attendance attendance = new()
            {
                EmployeeId = currentEmployeeId,
                CheckInTime = DateTime.Now.TimeOfDay
            };

            await _repository.AddAsync(attendance);
            await _repository.SaveChangesAsync();

            TempData["Message"] = "The attendance was successfully recorded";

            return RedirectToAction("MyAttendance");
        }

        [HttpGet]
        [Authorize(Roles = "Employee,Manager,HR")]
        public async Task<IActionResult> CheckOut()
        {
            var claimValue = User.FindFirst("EmployeeId")?.Value;
            int currentEmpId = 0;
            if(!string.IsNullOrEmpty(claimValue))
                int.TryParse (claimValue, out currentEmpId);

            var isCheckedIn = await _attendanceService.IsCheckedInAsync(currentEmpId, DateTime.UtcNow);
            if (isCheckedIn != null)
            {
                if (isCheckedIn.CheckOutTime == null)
                {
                    isCheckedIn.CheckOutTime = DateTime.Now.TimeOfDay;
                    _repository.Update(isCheckedIn);
                    await _repository.SaveChangesAsync();
                    TempData["Message"] = "The departure was successfully recorded";
                    return RedirectToAction("MyAttendance");
                }
                TempData["Message"] = "You have recorded the departure before";
                return RedirectToAction("MyAttendance");
            }

            TempData["Message"] = "You have not recorded attendance to record the departure";
            return RedirectToAction("MyAttendance");
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> TeamAttendance()
        {
            var cliamValue = User.FindFirst("EmployeeId")?.Value;
            if (!int.TryParse(cliamValue, out var ManagerId))
                return NotFound();

            int deptId = await _attendanceService.GetDepartmentIdByManagerIdAsync(ManagerId);

            var report = await _attendanceService.GetTeamAttendanceReportAsync(deptId);
            
            return View(report);
        }
    }
}
