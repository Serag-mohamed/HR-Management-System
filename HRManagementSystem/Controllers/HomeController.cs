using HRManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace HRManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly DashboardService _dashboardService;

        public HomeController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _dashboardService.GetDashboardDataAsync();
            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
