using HRManagementSystem.DTO;
using HRManagementSystem.Models;
using HRManagementSystem.Repositories;
using HRManagementSystem.Services;
using HRManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HRManagementSystem.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IRepository<Employee> _repository;
        private readonly EmployeeService _employeeService;

        public EmployeeController(IRepository<Employee> repository ,EmployeeService employeeService)
        {
            this._repository = repository;
            this._employeeService = employeeService;
        }

        private async Task PopulateDropdowns()
        {
            var positions = await _employeeService.GetPositionToAddEmployeeAsync();
            var managers = await _employeeService.GetManagerListDtoToAddEmployeesAsync();

            ViewBag.Positions = positions;
            ViewBag.Managers = managers;
        }


        [HttpGet]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetAllAsync();
            return View(employees);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Add()
        {
            await PopulateDropdowns();

            return View(new EmployeeViewModel());
        }



        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(EmployeeViewModel employeeViewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns();
                return View(employeeViewModel);
            }

            var employee = await _employeeService.GetByEmailWithIgnorFilterAsync(employeeViewModel.Email);

            if (employee != null && !employee.IsDeleted)
            {
                ModelState.AddModelError(nameof(employeeViewModel.Email), "This Employee already exists");
                await PopulateDropdowns();
                return View(employeeViewModel);
            }

            if (employee == null)
            {
                employee = new Employee();
            }
            else if (employee.IsDeleted)
            {
                employee.UndoDelete();
            }

            // تعيين الخصائص
            employee.FirstName = employeeViewModel.FirstName;
            employee.LastName = employeeViewModel.LastName;
            employee.Address = employeeViewModel.Address;
            employee.Email = employeeViewModel.Email;
            employee.Phone = employeeViewModel.Phone;
            employee.BasicSalary = employeeViewModel.BasicSalary;
            employee.HireDate = employeeViewModel.HireDate;
            employee.PositionId = employeeViewModel.PositionId;
            employee.ManagerId = employeeViewModel.ManagerId;

            if (employee.Id == 0)
            {
                await _repository.AddAsync(employee);
                await _repository.SaveChangesAsync();
                var user = new ApplicationUser()
                {
                    UserName = employee.Email,
                    Email = employee.Email,
                    EmployeeId = employee.Id,
                    DisplayName = employee.FullName
                };
                var defaultPassword = "Password@123";
                string role = await _employeeService.GetRoleAsync(employee.PositionId);
                var result = await _employeeService.CreateEmployeeUserAsync(user, defaultPassword, role);
                if (!result.Succeeded)
                {
                    foreach(var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);

                }
            }
            else
            {
                _repository.Update(employee);
                await _repository.SaveChangesAsync();
            }
                


            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "HR,Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee == null)
                return NotFound();
            await PopulateDropdowns();
            EmployeeViewModel employeeViewModel = new()
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Address = employee.Address,
                Email = employee.Email,
                Phone = employee.Phone,
                BasicSalary = employee.BasicSalary,
                HireDate = employee.HireDate,
                PositionId = employee.PositionId,
                ManagerId = employee.ManagerId,
            };
            return View(employeeViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "HR,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeViewModel employeeViewModel)
        {
            if (!ModelState.IsValid)
                return View(employeeViewModel);

            var employee = await _repository.GetByIdAsync(employeeViewModel.Id);
            if (employee == null)
                return NotFound();

            employee.FirstName = employeeViewModel.FirstName;
            employee.LastName = employeeViewModel.LastName;
            employee.Address = employeeViewModel.Address;
            employee.Email = employeeViewModel.Email;
            employee.Phone = employeeViewModel.Phone;
            employee.BasicSalary = employeeViewModel.BasicSalary;
            employee.HireDate = employeeViewModel.HireDate;
            employee.PositionId = employeeViewModel.PositionId;
            employee.ManagerId = employeeViewModel.ManagerId;

            _repository.Update(employee);
            await _repository.SaveChangesAsync();

            await _employeeService.UpdateEmployeeUserAsync(employee.Id, employee.Email, employee.FullName);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "HR,Admin,Manager,Employee")]
        public async Task<IActionResult> Details(int id)
        {
            var claimValue = User.FindFirst("EmployeeId")?.Value;
            int currentEmployeeId = 0;
            if (!string.IsNullOrEmpty(claimValue))
            {
                int.TryParse(claimValue, out currentEmployeeId);
            }


            if (User.IsInRole("Employee") && id != currentEmployeeId)
                return Forbid();

            EmployeeDetailsDto? employeeDetails = await _employeeService.GetEmployeeDetaitsAsync(id);
            if (employeeDetails == null)
                return NotFound();

            return View(employeeDetails);
        }


        [HttpGet]
        [Authorize(Roles = "HR,Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            _repository.Delete(id);
            await _repository.SaveChangesAsync();
            bool success = await _employeeService.BlockUserAccountAsync(id);
            if (!success)
                return NotFound();

            return RedirectToAction("Index");
        }


        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Team()
        {
            var cliamValue = User.FindFirst("EmployeeId")?.Value;
            if (!int.TryParse(cliamValue, out var ManagerId))
                return NotFound();

            int deptId = await _employeeService.GetDepartmentIdByManagerIdAsync(ManagerId);
            string? TeamName = await _employeeService.GetTeamNameByDepartmentIdAsync(deptId);
            ViewData["TeamName"] = TeamName;
            var teamEmployees = await _employeeService.GetTeamEmployeesByDepartmentIdAsync(deptId);
            return View(teamEmployees);
        }

    }
}
