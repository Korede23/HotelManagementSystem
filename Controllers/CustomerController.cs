using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Controllers
{

    public class CustomerController(ICustomerServices customerServices) : Controller
    {
        private readonly ICustomerServices _customerServices = customerServices;


        public async Task<IActionResult> Index()
        {
            var response = await _customerServices.GetAllCustomerCreatedAsync();
            if (response.Success)
            {
                return View(response.Data);
            }
            return View(new List<CustomerDto>());
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost("create-customer")]
        public async Task<IActionResult> CreateCustomer(CreateCustomer request)
        {

            var customer = await _customerServices.CreateCustomer(request);
            if (customer.Success)
            {
                return RedirectToAction("Index");
            }

            return BadRequest();


        }


        [HttpGet("edit-customer/{id}")]
        public async Task<IActionResult> EditCustomer([FromRoute] Guid id)
        {
            var customer = await _customerServices.GetCustomerAsync(id);

            return View(customer.Data);
        }

        [HttpPost("edit-customer/{id}")]
        public async Task<IActionResult> EditCustomer(UpdateCustomer request)
        {
            var customer = await _customerServices.UpdateCustomer(request.Id, request);
            if (customer.Success)
            {
                return RedirectToAction("Index");
            }
            return BadRequest();
        }


        [HttpGet("delete-customer/{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] Guid Id)
        {
            var customer = await _customerServices.DeleteCustomerAsync(Id);
            if (customer.Success)
            {
                return RedirectToAction("Index", "Customer");
            }

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
        public async Task<IActionResult> GetCustomerByIdAsync(Guid id)
        {
            var customer = await _customerServices.GetCustomerByIdAsync(id);
            if (customer.Success)
            {
                return View(customer);
            }

            return BadRequest();

        }

    }
}


