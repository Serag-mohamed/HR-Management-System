using HRManagementSystem.Models;
using HRManagementSystem.Repositories;
using HRManagementSystem.Services;
using HRManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRManagementSystem.Controllers
{

    public class PositionController : Controller
    {
        private readonly IRepository<Position> _repository;
        private readonly PositionService _positionService;

        public PositionController(IRepository<Position> repository,PositionService positionService)
        {
            this._repository = repository;
            this._positionService = positionService;
        }
        [Authorize(Roles = "HR,Admin")]
        public async Task<IActionResult> Index()
        {
            var positions = await _positionService.GetAllAsync();
            return View(positions);
        }

        [HttpGet]
        [Authorize(Roles = "HR,Admin")]
        public async Task<IActionResult> Add()
        {
            var departments = await _positionService.GetPositionsForAddEmployeeAsync();
            ViewBag.Departments = departments;
            return View(new AddPositionViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "HR,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddPositionViewModel positionViewModel)
        {
            if (ModelState.IsValid)
            {
                Position? position = await _repository.GetByNameWithIgnorFilterAsync(positionViewModel.Name);

                if (position == null)
                {
                    position = new Position
                    {
                        Name = positionViewModel.Name,
                        Description = positionViewModel.Description,
                        DepartmentId = positionViewModel.DepartmentId
                    };

                    await _repository.AddAsync(position);
                    await _repository.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else if (position.IsDeleted)
                {
                    position.UndoDelete();
                    position.Description = positionViewModel.Description;
                    position.DepartmentId = positionViewModel.DepartmentId;

                    _repository.Update(position);
                    await _repository.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "This position already exists");
                }
            }

            ViewBag.Departments = await _positionService.GetPositionsForAddEmployeeAsync();
            return View(positionViewModel);
        }


        [Authorize(Roles = "HR,Admin")]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var position = await _positionService.GetPositionWithEmployeesAsync(id);
            if (position == null) return NotFound();
            return View(position);
        }

        [Authorize(Roles = "HR,Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Position? position = await _repository.GetByIdAsync(id);
            if (position == null)
                return NotFound();
            AddPositionViewModel positionViewModel = new()
            {
                Id = id,
                Name = position.Name,
                Description = position.Description
            };
            return View(positionViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "HR,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AddPositionViewModel positionViewModel)
        {
            if (ModelState.IsValid)
            {
                Position? position = await _repository.GetByIdAsync(positionViewModel.Id);
                if (position == null)
                    return NotFound();
                position.Name = positionViewModel.Name;
                position.Description = positionViewModel.Description;
                _repository.Update(position);
                await _repository.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(positionViewModel);
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

