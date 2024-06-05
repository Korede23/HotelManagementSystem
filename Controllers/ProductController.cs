using HMS.Implementation.Services;
using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Implementation.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductServices _productServices;

        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        [HttpGet("get-product")]
        public async Task<IActionResult> Index()
        {
            var product = await _productServices.GetAllProductAsync();
            return View(product.Data);
            //return View(new List<ProductDto>());
        }
       

        public async Task<IActionResult> Create()
        {
            var product = await _productServices.GetAllProductAsync();
            if (product.Success)
            {
               
                return View();
            }
            return BadRequest();
        }



        [HttpPost("create-product")]
        public async Task<IActionResult> CreateProduct(CreateProduct request)
        {
            var product = await _productServices.CreateProduct(request);
            if (product.Success)
            {
                return RedirectToAction("Index");
            }
            return BadRequest();
        }


        [HttpGet("edit-product/{id}")]
        public async Task<IActionResult> EditProduct([FromRoute] Guid id)
        {
            var product = await _productServices.GetProductAsync(id);

            return View(product.Data);
        }


        [HttpPost("update-product")]
        public async Task<IActionResult> UpdateProduct(UpdateProduct request)
        {

            var product = await _productServices.UpdateProduct(request.Id , request);
            if (product.Success)
            {
                return RedirectToAction("Index");
            }
            return View(request);
        }




        [HttpGet("delete-product/{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid Id)
        {
            var product = await _productServices.DeleteProductAsync(Id);
            if (product.Success)
            {
                return RedirectToAction("Index", "Product");
            }

            return BadRequest(product);

        }


        [HttpGet("get-all-product-created")]
        public async Task<IActionResult> GetAllBookingsAsync()
        {
            var product = await _productServices.GetAllProductAsync();
            if (product.Success)
            {
                return View(product);
            }
            else
            {
                return BadRequest(product);
            }


        }

        [HttpGet("get-product-by-id/{id}")]
        public async Task<IActionResult> GetAllProductById(Guid id)
        {
            var product = await _productServices.GetAllProductsByIdAsync(id);
            if (product.Success == false)
            {
                return View(product);
            }
            else
            {
                return BadRequest(product);
            }
        }


    }
}
