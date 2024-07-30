using AspNetCoreHero.ToastNotification.Abstractions;
using HMS.Implementation.Services;
using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Implementation.Services;
using HotelManagementSystem.Model.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Controllers
{
    [Authorize]
    public class CustomerStatusController : Controller
    {
        private readonly ICustomerStatusServices _customerStatusServices;
        private readonly INotyfService _notyf;

        public CustomerStatusController(ICustomerStatusServices customerStatusServices , INotyfService notyf) 
        {
            _customerStatusServices = customerStatusServices;
            _notyf = notyf;
        }

        [HttpGet("get-customerStatus")]
        public async Task<IActionResult> CustomerStatus()
        {
            var status = await _customerStatusServices.GetCustomerStatus();
            return View(status);
        }


        [HttpGet("check-in")]
        public IActionResult CheckIn()
        {
            var customers = _customerStatusServices.GetCustomerSelect();
            ViewBag.Customers = new SelectList(customers, "Id", "Name");
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
            return View();
        }

        [HttpGet("check-out/{customerId}")]
        public IActionResult CheckOut()
        {
            var customerStatus = _customerStatusServices.GetSelectCustomerCheckedIn();
            ViewBag.CustomerStatus = new SelectList(customerStatus, "Id", "Name");
            return View();
        }

        [HttpPost("check-out/{customerId}")]
        public async Task<IActionResult> CheckOut([FromRoute] Guid customerId)
        {
            var response = await _customerStatusServices.CheckOut(customerId);
            if (response.Success)
            {
                _notyf.Success(response.Message, 3);
                return RedirectToAction("CustomerStatus");
            }
            _notyf.Error(response.Message);
            return BadRequest();
        }
    }

}

