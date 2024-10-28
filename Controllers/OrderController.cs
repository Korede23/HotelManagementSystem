using AspNetCoreHero.ToastNotification.Abstractions;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace HotelManagementSystem.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;
        private readonly IProductServices _productServices;
        private readonly INotyfService _notyf;

        public OrderController(IOrderServices orderServices, IProductServices productServices, INotyfService notyf)
        {
            _orderServices = orderServices;
            _productServices = productServices;
            _notyf = notyf;
        }


        [HttpGet("get-order")]
        public async Task<IActionResult> Orders()
        {
            var order = await _orderServices.GetOrders();
            return View(order);
            // return View(new List<OrderDto>());
        }

        [HttpGet("create-order")]
        public async Task<IActionResult> CreateOrder(Guid productId)
        {
            var products = await _productServices.GetAllProductsByIdAsync(productId);
            if (products.Success)
            {
                var product = products.Data.Name;
                ViewBag.ProductName = product;
            }
            return View();
        }




        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder(CreateOrder request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (Guid.TryParse(userId, out var customerId))
            {
                request.UserId = customerId.ToString();

                var order = await _orderServices.CreateOrder(request);
                if (order.Success)
                {
                    _notyf.Success(order.Message, 3);
                    var productId = order.Data;
                    return RedirectToAction("InitiatePaymentForm", "Payment", new { userId = customerId, productId });
                }
                else
                {
                    _notyf.Error(order.Message, 3);
                    return RedirectToAction("GetProducts", "Product");
                }
            }
            return RedirectToAction("GetProducts");
        }


        [HttpGet("edit-order/{id}")]
        public async Task<IActionResult> EditOrder([FromRoute] Guid id)
        {
            var order = await _orderServices.GetOrderAsync(id);
            var products = _orderServices.GetProductSelect();
            ViewBag.Products = new SelectList(products, "Id", "ProductName");

            return View(order.Data);
        }


        [HttpPost("edit-order/{id}")]
        public async Task<IActionResult> EditOrder(UpdateOrder request)
        {

            var order = await _orderServices.UpdateOrder(request.Id, request);
            if (order.Success)
            {
                _notyf.Success(order.Message, 3);
                return RedirectToAction("Orders");
            }
            _notyf.Error(order.Message);
            return View(request);
        }




        [HttpGet("delete-order/{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] Guid id)
        {
            var order = await _orderServices.DeleteOrderAsync(id);
            if (order.Success)
            {
                _notyf.Success(order.Message, 3);
                return RedirectToAction("Orders", "Order");
            }
            _notyf.Error(order.Message);
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

        [HttpGet("get-order/{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _orderServices.GetOrderByIdAsync(id);
            if (order != null)
            {
                _notyf.Success(order.Message, 3);
                return View(order.Data);
            }
            _notyf.Error(order?.Message);
            return RedirectToAction("Orders");
        }



    }
}



