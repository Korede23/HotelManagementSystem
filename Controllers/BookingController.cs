using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelManagementSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingServices _bookService;
        private readonly IRoomService _roomService;

        public BookingController(IBookingServices bookService, IRoomService roomService)
        {
            _bookService = bookService;
            _roomService = roomService;
        }

        [HttpGet("get-booking")]
        public async Task<IActionResult> Index()
        {
            var response = await _bookService.GetBooking();
            return View(response);
        }



        [HttpGet("create-booking")]
        public IActionResult Create()
        {
            var selectRoom = _bookService.GetRoomSelect();
            if (selectRoom == null)
            {
                selectRoom = new List<SelectRoomDto>();
            }
            ViewData["SelectRoom"] = new SelectList(selectRoom, "Id", "RoomName");
            return View();
        }



        [HttpPost("create-booking")]
        public async Task<IActionResult> CreateBooking(CreateBooking request, Guid Id)
        {
            var booking = await _bookService.CreateBooking(request, Id);
            if (booking.Success)
            {
                return RedirectToAction("Index");
            }
            return BadRequest();
        }


        [HttpGet("edit-booking/{id}")]
        public async Task<IActionResult> EditBooking([FromRoute] Guid id)
        {
            var selectRoom = _bookService.GetRoomSelect();
            if (selectRoom == null)
            {
                selectRoom = new List<SelectRoomDto>();
            }
            ViewData["SelectRoom"] = new SelectList(selectRoom, "Id", "RoomName");
            return View();
        }

       


        [HttpPost("edit-booking/{id}")]
        public async Task<IActionResult> EditBooking(UpdateBooking request)
        {

            var booking = await _bookService.UpdateBooking(request.Id, request);
            if (booking.Success)
            {
                return RedirectToAction("Index");
            }
            return View("Booking");
        }




        [HttpGet("delete-booking/{id}")]
        public async Task<IActionResult> DeleteBooking([FromRoute] Guid id)
        {
            var booking = await _bookService.DeleteBookingAsync(id);
            if (booking.Success)
            {
                return RedirectToAction("Index", "Booking");
            }

            return BadRequest(booking);

        }


        [HttpGet("get-all-booking-created")]
        public async Task<IActionResult> GetAllBookingsAsync()
        {
            var bookings = await _bookService.GetAllBookingsAsync();
            if (bookings.Success == false)
            {
                return Ok(bookings);
            }
            else
            {
                return BadRequest(bookings);
            }


        }

        [HttpGet("get-booking-by-id/{id}")]
        public async Task<IActionResult> GetBookingByIdAsync(Guid id)
        {
            var bookings = await _bookService.GetBookingByIdAsync(id);
            if (bookings.Success == false)
            {
                return Ok(bookings);
            }
            else
            {
                return BadRequest(bookings);
            }
        }


    }
}
