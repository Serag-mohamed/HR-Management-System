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
    public class DepartmentController : Controller
    {
        private readonly IRepository<Department> _repository;
        private readonly DepartmentService _departmentService;

        public DepartmentController(IRepository<Department> repository, DepartmentService departmentService)
        {
            this._repository = repository;
            this._departmentService = departmentService;
        }

        [HttpGet]
        [Authorize(Roles = "HR,Admin")]
        public async Task<IActionResult> Index()
        {
            List<DepartmentDto> depts = await _departmentService.GetAllAsync();
            return View(depts);
        }

        [HttpGet]
        [Authorize(Roles = "HR,Admin")]
        public IActionResult Add() 
        { 
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "HR,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(DepartmentViewModel departmentViewModel)
        {
            if (ModelState.IsValid)
            {
                Department? department = await _repository.GetByNameWithIgnorFilterAsync(departmentViewModel.Name);

                if (department == null)
                {
                    department = new Department
                    {
                        Name = departmentViewModel.Name,
                    };

                    await _repository.AddAsync(department);
                    await _repository.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else if (department.IsDeleted)
                {
                    department.UndoDelete();

                    _repository.Update(department);
                    await _repository.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                    ModelState.AddModelError(" ", "This department already exists");
            }

            return View(departmentViewModel);
        }

        [Authorize(Roles = "HR,Admin")]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var dept = await _departmentService.GetDepartmentWithPositionsAsync(id);
            return View(dept);
        }

        [Authorize(Roles = "HR,Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Department? department = await _repository.GetByIdAsync(id);
            if (department == null)
                return NotFound();
            DepartmentViewModel departmentViewModel = new()
            {
                Id = id,
                Name = department.Name,
            };
            return View(departmentViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "HR,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepartmentViewModel departmentViewModel)
        {
            if (ModelState.IsValid)
            {
                Department department = await _repository.GetByIdAsync(departmentViewModel.Id);
                department.Name = departmentViewModel.Name;
                _repository.Update(department);
                 await _repository.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(departmentViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "HR,Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            _repository.Delete(id);
            await _repository.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
