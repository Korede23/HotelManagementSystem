using AspNetCoreHero.ToastNotification.Abstractions;
using HMS.Implementation.Interface;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Implementation.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelManagementSystem.Controllers
{
    [Authorize]
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IAmenityService _amenityService;
        private readonly INotyfService _notyf;

        public RoomController(IRoomService roomService, IAmenityService amenityService, INotyfService notyf)
        {
            _roomService = roomService;
            _amenityService = amenityService;
            _notyf = notyf;
        }

        [HttpGet("get-rooms")]
        public async Task<IActionResult> Rooms()
        {
            var room = await _roomService.GetAllRoomsCreatedAsync();
            return View(room);
        }

        [HttpGet("create-room")]
        public IActionResult CreateRoom()
        {
            var selectAmenity = _roomService.GetAmenitySelect();

            if (selectAmenity == null)
            {
                selectAmenity = new List<SelectAmenityDto>();
            }
            ViewData["SelectAmenity"] = new SelectList(selectAmenity, "Id", "AmenityName");
            return View();

        }


        [HttpPost("create-room")]
        public async Task<IActionResult> CreateRoom(CreateRoom request)
        {
            var room = await _roomService.CreateRoom(request);
            if (room.Success)
            {
                _notyf.Success(room.Message, 3);
                return RedirectToAction("Rooms");
            }
            _notyf.Error(room.Message, 3);
            return BadRequest();

        }

        [HttpGet("edit-room/{id}")]
        public async Task<IActionResult> EditRoom([FromRoute] Guid id)
        {
            var room = await _roomService.GetRoomAsync(id);

            var selectAmenity = _roomService.GetAmenitySelect();
            ViewData["SelectAmenity"] = new SelectList(selectAmenity, "Id", "AmenityName");

            return View(room.Data);

        }

        [HttpPost("Update-room")]
        public async Task<IActionResult> UpdateRoom(UpdateRoom request)
        {
            var room = await _roomService.UpdateRoom(request.Id, request);
            if (room.Success)
            {
                _notyf.Success(room.Message, 3);
                return RedirectToAction("Rooms");

            }
            _notyf.Error(room.Message, 3);
            return View(request);
        }


        [HttpGet("delete-room/{id}")]
        public async Task<IActionResult> DeleteRoom([FromRoute] Guid id)
        {
            var response = await _roomService.DeleteRoomAsync(id);
            if (response.Success)
            {
                _notyf.Success(response.Message, 3);
                return RedirectToAction("Rooms", "Room");
            }

            _notyf.Error(response.Message, 3);
            return RedirectToAction("Error", new { message = response.Message });
        }



        [HttpGet("room/{id}")]
        public async Task<IActionResult> GetRoomsById(Guid id)
        {
            var rooms = await _roomService.GetRoomsByIdAsync(id);
            if (rooms != null)
            {
                _notyf.Success(rooms.Message, 3);
                return View(rooms.Data);
            }
            _notyf.Error(rooms?.Message);
            return RedirectToAction("Rooms");
        }
    }
}
