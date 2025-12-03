using HRManagementSystem.Enums;
using HRManagementSystem.Models;
using HRManagementSystem.Repositories;
using HRManagementSystem.Services;
using HRManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HRManagementSystem.Controllers
{
    public class LeaveController : Controller
    {
        private readonly IRepository<Leave> _repository;
        private readonly LeaveService _leaveService;

        public LeaveController(IRepository<Leave> repository, LeaveService leaveService)
        {
            this._repository = repository;
            this._leaveService = leaveService;
        }
        public async Task<IActionResult> Index()
        {
            var leaves = await _leaveService.GetAllLeavesAsync();
            return View(leaves);
        }

        [HttpGet]
        [Authorize(Roles = "Employee,HR,Manager")]
        public async Task<IActionResult> MyLeaves()
        {
            var claimValue = User.FindFirst("EmployeeId")?.Value;
            int currentEmployeeId = 0;
            if (!string.IsNullOrEmpty(claimValue))
            {
                int.TryParse(claimValue, out currentEmployeeId);
            }

            var leaves = await _leaveService.GetLeavesForEmployeeAsync(currentEmployeeId);

            return View(leaves);
        }

        [HttpGet]
        [Authorize(Roles = "Employee,HR,Manager")]
        public IActionResult Add()
        {
            return View(new LeaveViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Employee,HR,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(LeaveViewModel leaveViewModel)
        {
            if (!ModelState.IsValid)
                return View(leaveViewModel);

            var isLeaveExisted = await _leaveService.IsExistedLeave(leaveViewModel.StartDate, leaveViewModel.EndDate);
            if (isLeaveExisted)
            {
                TempData["Message"] = "There is already a leave during this period.";
                return RedirectToAction("Add");
            }

            var employeeIdClaim = User.FindFirst("EmployeeId")?.Value;
            if (string.IsNullOrEmpty(employeeIdClaim) || !int.TryParse(employeeIdClaim, out int employeeId))
            {
                TempData["Message"] = "Invalid employee information.";
                return RedirectToAction("Add");
            }

            var leave = new Leave
            {
                EmployeeId = employeeId,
                LeaveType = leaveViewModel.LeaveType,
                StartDate = leaveViewModel.StartDate,
                EndDate = leaveViewModel.EndDate,
                RequestDate = DateOnly.FromDateTime(DateTime.Now),
                Status = LeaveStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(leave);
            await _repository.SaveChangesAsync();

            return RedirectToAction("MyLeaves");
        }

        [HttpGet]
        [Authorize(Roles = "Employee,HR,Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var leave = await _repository.GetByIdAsync(id);
            if (leave == null)
            {
                return NotFound();
            }
            var leaveViewModel = new LeaveViewModel
            {
                LeaveType = leave.LeaveType,
                StartDate = leave.StartDate,
                EndDate = leave.EndDate
            };
            return View(leaveViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Employee,HR,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveViewModel leaveViewModel)
        {
            if (!ModelState.IsValid)
                return View(leaveViewModel);

            var leave = await _repository.GetByIdAsync(id);

            if (leave == null)
                return NotFound();

            leave.LeaveType = leaveViewModel.LeaveType;
            leave.StartDate = leaveViewModel.StartDate;
            leave.EndDate = leaveViewModel.EndDate;
            _repository.Update(leave);
            await _repository.SaveChangesAsync();

            return RedirectToAction("MyLeaves");
        }

        [HttpGet]
        [Authorize(Roles = "Employee,HR,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            _repository.Delete(id);
            await _repository.SaveChangesAsync();
            return RedirectToAction("MyLeaves");
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Approve()
        {
            var cliamValue = User.FindFirst("EmployeeId")?.Value;
            if (!int.TryParse(cliamValue, out var ManagerId))
                return NotFound();

            int deptId = await _leaveService.GetDepartmentIdByManagerIdAsync(ManagerId);
            var teamLeaves = await _leaveService.GetTeamLeavesByDeptIdAsync(deptId);

            return View(teamLeaves);
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Accept (int id)
        {
            var leave = await _repository.GetByIdAsync(id);
            if (leave == null)
                return NotFound();
            leave.Status = LeaveStatus.Approved;
            _repository.Update(leave);
            await _repository.SaveChangesAsync();
            return RedirectToAction("Approve");
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Reject(int id)
        {
            var leave = await _repository.GetByIdAsync(id);
            if (leave == null)
                return NotFound();
            leave.Status = LeaveStatus.Rejected;
            _repository.Update(leave);
            await _repository.SaveChangesAsync();
            return RedirectToAction("Approve");
        }
    }
}

