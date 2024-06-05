using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Implementation.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers
{

    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;
        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
           
        }


        [HttpGet("get-order")]
        public async Task<IActionResult> Index()
        {
            var order = await _orderServices.GetOrder();
            return View(order);
           // return View(new List<OrderDto>());
        }

        
        public async Task<IActionResult> Create()
        {
            var order = await _orderServices.GetAllOrderAsync();
            if (order.Success)
            {
                
                return View();
            }
            return BadRequest();
        }



        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder(CreateOrder request)
        {
            var order = await _orderServices.CreateOrder(request);
            if (order.Success)
            {
                return RedirectToAction("Index");
            }
            return BadRequest();
        }


        [HttpGet("edit-order/{id}")]
        public async Task<IActionResult> EditOrder([FromRoute] Guid id)
        {
            var order = await _orderServices.GetOrderAsync(id);

            return View(order.Data);
        }


        [HttpPost("edit-order/{id}")]
        public async Task<IActionResult> EditOrder(UpdateOrder request)
        {

            var order = await _orderServices.UpdateOrder(request.CustomerId, request);
            if (order.Success)
            {
                return RedirectToAction("Index");
            }
            return View(request);
        }




        [HttpGet("delete-order/{id}")]
        public async Task<IActionResult> Deleteorder([FromRoute] Guid id)
        {
            var order = await _orderServices.DeleteOrderAsync(id);
            if (order.Success)
            {
                return RedirectToAction("Index", "Order");
            }

            return BadRequest(order);

        }


        [HttpGet("get-all-order-created")]
        public async Task<IActionResult> GetAllOrderAsync()
        {
            var order = await _orderServices.GetAllOrderAsync();
            if (order.Success)
            {
                return View(order);
            }
            else
            {
                return BadRequest(order);
            }


        }

        [HttpGet("get-order-by-id/{id}")]
        public async Task<IActionResult> GetAllOrderById(Guid Id)
        {
            var order = await _orderServices.GetOrderByIdAsync(Id);
            if (order.Success)
            {
                return View(order);
            }
            else
            {
                return BadRequest(order);
            }
        }


    }
}



