using HotelManagementSystem.Dto;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Implementation.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly IDashBoardService _dashBoardService;

        public DashBoardController(IDashBoardService dashBoardService)
        {
            _dashBoardService = dashBoardService;
        }
        public ActionResult<DashBoardDto> GetDashboardCounts()
        {
            var result = _dashBoardService.DashBoardCount();
            return View(result);
        }
    }
}
