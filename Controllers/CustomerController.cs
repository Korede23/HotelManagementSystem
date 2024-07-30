using AspNetCoreHero.ToastNotification.Abstractions;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Implementation.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers
{
    [Authorize]
    public class CustomerController(ICustomerServices customerServices, INotyfService notyf) : Controller
    {
        private readonly ICustomerServices _customerServices = customerServices;
        private readonly INotyfService _notyf = notyf;
        [AllowAnonymous]
        [HttpGet("get-customer")]
        public async Task<IActionResult> Customers()
        {
            var customer = await _customerServices.GetCustomer();
            return View(customer);
        }
        [AllowAnonymous]
        [HttpGet("create-customer")]
        public async Task<IActionResult> CreateCustomer()
        {
            var response = await _customerServices.GetAllCustomerCreatedAsync();
            if (response.Success)
            {
                return View();
            }
            return BadRequest();
        }





        [HttpGet("edit-customer/{id}")]
        public async Task<IActionResult> EditCustomer([FromRoute] string id)
        {
            var customer = await _customerServices.GetCustomerAsync(id);
            _notyf.Success(customer.Message, 3);
            return View(customer.Data);
        }

        [HttpPost("edit-customer/{id}")]
        public async Task<IActionResult> EditCustomer(UpdateCustomer request)
        {
            var customer = await _customerServices.UpdateCustomer(request.Id.ToString(), request);
            if (customer.Success)
            {
                _notyf.Success(customer.Message, 3);
                return RedirectToAction("Customers");
            }
            _notyf.Error(customer.Message);
            return BadRequest();
        }


        public IActionResult Login()
        {
            return View();
        }




        [HttpGet("delete-customer/{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] string Id)
        {
            var customer = await _customerServices.DeleteCustomerAsync(Id);
            if (customer.Success)
            {
                _notyf.Success(customer.Message, 3);
                return RedirectToAction("Customers", "Customer");
            }
            _notyf.Error(customer.Message);
            return BadRequest(customer);

        }


        [HttpGet("get-all-user-customer")]
        public async Task<IActionResult> GetAllCustomer()
        {
            var customer = await _customerServices.GetAllCustomerCreatedAsync();
            if (customer.Success)
            {
                return View(customer);
            }

            return BadRequest();


        }



        [HttpGet("get-customer-by-id/{id}")]
        public async Task<IActionResult> GetCustomerById(string id)
        {
            var customer = await _customerServices.GetCustomerByIdAsync(id);
            if (customer.Success)
            {
                _notyf.Success(customer.Message, 3);
                return View(customer.Data);
            }
            _notyf.Error(customer.Message);
            return RedirectToAction("Customers");

        }

    }
}


