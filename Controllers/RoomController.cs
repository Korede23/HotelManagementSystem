using HMS.Implementation.Interface;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Implementation.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelManagementSystem.Controllers
{
    public class RoomController : Controller
    {


        private readonly IRoomService _roomService;
        private readonly IAmenityService _amenityService;

        public RoomController(IRoomService roomService, IAmenityService amenityService)
        {
            _roomService = roomService;
            _amenityService = amenityService;
        }

        [HttpGet("get-room")]
        public async Task<IActionResult> Index()
        {
            var room = await _roomService.GetRoom();
            return View(room);
        }

        [HttpGet("select-amenity")]
        public async Task<IActionResult> Create()
        {
            var amenities = await _amenityService.GetAllAmenity();

            if (amenities.Data != null)
            {
                ViewBag["SelectAmenity"] = new SelectList(amenities.Data, "Id", "Name");
            }
            return View();
        }


        [HttpPost("create-room")]
        public async Task<IActionResult> CreateRoom(CreateRoom request)
        {
            var room = await _roomService.CreateRoom(request);
            if (room.Success)
            {
                return RedirectToAction("Index");
            }
            return BadRequest();

        }

        [HttpGet("edit-room/{id}")]
        public async Task<IActionResult> EditRoom([FromRoute] Guid id)
        {
            var room = await _roomService.GetRoomAsync(id);

            return View(room.Data);
        }

        [HttpPost("Update-room")]
        public async Task<IActionResult> UpdateRoom(UpdateRoom request)
        {
            var room = await _roomService.UpdateRoom(request.Id, request);
            if (room.Success)
            {
                return RedirectToAction("Index");

            }

            return View(request);
        }



        [HttpGet("delete-room/{id}")]
        public async Task<IActionResult> DeleteRoom([FromRoute] Guid Id)
        {
            var room = await _roomService.DeleteRoomAsync(Id);
            if (room.Success)
            {
                return RedirectToAction("Index","Room");
            }
            return BadRequest(room);
        }


        [HttpGet("get-all-rooms-created")]
        public async Task<IActionResult> GetAllRoomsCreatedAsync()
        {
            var rooms = await _roomService.GetAllRoomsCreatedAsync();
            if (rooms.Success)
            {
                return View(rooms);
            }
            return BadRequest(rooms);
        }

        [HttpGet("get-room-by-id/{id}")]
        public async Task<IActionResult> GetRoomsByIdAsync(Guid id)
        {
            var rooms = await _roomService.GetRoomsByIdAsync(id);
            if (rooms!= null)
            {
                return View(rooms);
            }
            return BadRequest(rooms);
        }
    }




}



