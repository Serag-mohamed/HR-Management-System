using HRManagementSystem.DTO;
using HRManagementSystem.Models;
using HRManagementSystem.Repositories;
using HRManagementSystem.Services;
using HRManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HRManagementSystem.Controllers
{
    public class PayrollController : Controller
    {
        private readonly IRepository<Payroll> _repository;
        private readonly PayrollService _payrollService;

        public PayrollController(IRepository<Payroll> repository, PayrollService payrollService)
        {
            this._repository = repository;
            this._payrollService = payrollService;
        }
        [HttpGet]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Index()
        {
            var payrolls = await _payrollService.GetAllPayrolls();
            return View(payrolls);
        }

        [HttpGet]
        [Authorize(Roles = "HR,Manager,Employee")]
        public async Task<IActionResult> MyPayroll()
        {
            var claimValue = User.FindFirst("EmployeeId")?.Value;

            if (!int.TryParse(claimValue, out int empId))
                return View(new List<PayrollDetailsDto>());

            var payrolls = await _payrollService.GetPayrollByEmployeeId(empId);
            return View(payrolls);
        }

        [HttpGet]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> Add()
        {
            var Employees = await _payrollService.GetListOfEmployee();
            ViewBag.Employees = Employees;
            return View(new PayrollViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "HR")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PayrollViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var Employees = await _payrollService.GetListOfEmployee();
                ViewBag.Employees = Employees;
                return View(model);
            }

            var PayrollExists = await _payrollService.CheckPayrollExists(model.EmployeeId, model.PayrollMonth);
            if (PayrollExists)
            {
                ModelState.AddModelError("", "The Payroll has been added to this employee befor.");
                var Employees = await _payrollService.GetListOfEmployee();
                ViewBag.Employees = Employees;
                return View(model);
            }

            Payroll payroll = new()
            {
                EmployeeId = model.EmployeeId,
                BasicSalary = model.BasicSalary,
                Bonuses = model.Bonuses,
                Deductions = model.Deductions,
                NetSalary = model.BasicSalary + model.Bonuses - model.Deductions,
                PayrollMonth = model.PayrollMonth,
            };

            await _repository.AddAsync(payroll);
            await _repository.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetBasicSalary(int employeeId)
        {
            if (employeeId <= 0)
            {
                return NotFound(new { message = "Employee ID is invalid." });
            }

            
            var employee = await _payrollService.GetEmployeeByIdAsync(employeeId);

            if (employee == null)
            {
                return NotFound(new { message = "Employee not found." });
            }

            return Json(new { salary = employee.BasicSalary });
        }

        [HttpGet]
        [Authorize(Roles = "HR,Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var payroll = await _repository.GetByIdAsync(id);
            if (payroll == null)
                return View("Index");

            PayrollViewModel payrollViewModel = new()
            {
                Id = payroll.Id,
                EmployeeId = payroll.EmployeeId,
                BasicSalary = payroll.BasicSalary,
                Bonuses = payroll.Bonuses,
                Deductions = payroll.Deductions,
                PayrollMonth = payroll.PayrollMonth,
            };

            var Employees = await _payrollService.GetListOfEmployee();
            ViewBag.Employees = Employees;

            return View(payrollViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "HR,Admin")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(PayrollViewModel payrollViewModel)
        {
            if (!ModelState.IsValid)
                return View(payrollViewModel);

            var payroll = await _repository.GetByIdAsync(payrollViewModel.Id);
            if (payroll == null)
                return NotFound();

            payroll.EmployeeId = payrollViewModel.EmployeeId;
            payroll.BasicSalary = payrollViewModel.BasicSalary;
            payroll.Bonuses = payrollViewModel.Bonuses;
            payroll.Deductions = payrollViewModel.Deductions;
            payroll.NetSalary = payrollViewModel.BasicSalary + payrollViewModel.Bonuses - payrollViewModel.Deductions;
            payroll.PayrollMonth = payrollViewModel.PayrollMonth;

            _repository.Update(payroll);
            await _repository.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
