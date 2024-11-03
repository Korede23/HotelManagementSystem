using AspNetCoreHero.ToastNotification.Abstractions;
using HotelManagementSystem.Implementation.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace HotelManagementSystem.Controllers
{
    [Authorize]
    public class CustomerStatusController : Controller
    {
        private readonly ICustomerStatusServices _customerStatusServices;
        private readonly IBookingServices _bookingServices;
        private readonly INotyfService _notyf;

        public CustomerStatusController(ICustomerStatusServices customerStatusServices , IBookingServices bookingServices , INotyfService notyf) 
        {
            _customerStatusServices = customerStatusServices;
            _bookingServices = bookingServices;
            _notyf = notyf;
        }

        [HttpGet("get-customerStatus")]
        public async Task<IActionResult> CustomerStatus()
        {
            var status = await _customerStatusServices.GetCustomerStatus();
            return View(status);
        }


        [HttpGet("check-in")]
        public async Task<IActionResult> CheckIn()
        {
            var activeBookings = await _bookingServices.GetActiveBookings();
            ViewBag.ActiveBookings = new SelectList(activeBookings, "BookingId", "CustomerName" , "CustomerId");
            var getooking = activeBookings.FirstOrDefault();
            ViewBag.CustomerId = getooking?.CustomerId;
            return View();
        }


        [HttpPost("check-in")]
        public async Task<IActionResult> CheckIn(string customerId, Guid bookingId)
        {
            var response = await _customerStatusServices.CheckIn(customerId, bookingId);
            if (response.Success)
            {
                _notyf.Success(response.Message, 3);
                return RedirectToAction("CustomerStatus");
            }

            ModelState.AddModelError(string.Empty, response.Message);
            _notyf.Error(response.Message);
            return RedirectToAction("CustomerStatus");
        }

        [HttpGet("check-out/{customerId}")]
        public IActionResult CheckOut()
        {
            var customerStatus = _customerStatusServices.GetSelectCustomerCheckedIn();
            ViewBag.CustomerStatus = new SelectList(customerStatus, "Id", "Name");
            return View();
        }

        [HttpPost("check-out/{customerId}")]
        public async Task<IActionResult> CheckOut([FromRoute] string customerId)
        {
            var response = await _customerStatusServices.CheckOut(customerId);
            if (response.Success)
            {
                _notyf.Success(response.Message, 3);
                return RedirectToAction("CustomerStatus");
            }
            _notyf.Error(response.Message);
            return RedirectToAction("CustomerStatus");
        }
    }

}

