using AspNetCoreHero.ToastNotification.Abstractions;
using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelManagementSystem.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IBookingServices _bookService;
        private readonly IRoomService _roomService;
        private readonly INotyfService _notyf;

        public BookingController(IBookingServices bookService, IRoomService roomService , INotyfService notyf)
        {
            _bookService = bookService;
            _roomService = roomService;
            _notyf = notyf;
        }

        [HttpGet("get-booking")]
        public async Task<IActionResult> Bookings()
        {
            var book = await _bookService.GetBooking();
            return View(book);
        }



        [HttpGet("create-booking")]
        public IActionResult CreateBooking()
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
        public async Task<IActionResult> CreateBooking(CreateBooking request)
        {
            var booking = await _bookService.CreateBooking(request);
            if (booking.Success)
            {
                _notyf.Success(booking.Message, 3);
                return RedirectToAction("Bookings");
            }
            _notyf.Error(booking.Message);
            return BadRequest();
        }



        [HttpGet("edit-booking/{id}")]
        public IActionResult EditBooking([FromRoute] Guid id)
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
                _notyf.Success(booking.Message, 3);
                return RedirectToAction("Bookings");
            }
            _notyf.Error(booking.Message);
            return View("Booking");
        }




        [HttpGet("delete-booking/{id}")]
        public async Task<IActionResult> DeleteBooking([FromRoute] Guid id)
        {
            var booking = await _bookService.DeleteBookingAsync(id);
            if (booking.Success)
            {
                _notyf.Success(booking.Message, 3);
                return RedirectToAction("Bookings", "Booking");
            }
            _notyf.Error(booking.Message);
            return BadRequest(booking);

        }


        [HttpGet("get-all-booking-created")]
        public async Task<IActionResult> GetAllBookingsAsync()
        {
            var bookings = await _bookService.GetAllBookingsAsync();
            if (bookings.Success)
            {
                return View(bookings);
            }
                return BadRequest(bookings);
        }

        [HttpGet("get-booking-by-id/{id}")]
        public async Task<IActionResult> GetBookingById(Guid id)
        {
            var bookings = await _bookService.GetBookingByIdAsync(id);
            if (bookings != null)
            {
                _notyf.Success(bookings.Message, 3);
                return View(bookings.Data);
            }
            _notyf.Error(bookings?.Message);
            return RedirectToAction("Bookings");
        }

       

    }
}
