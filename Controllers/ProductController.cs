using AspNetCoreHero.ToastNotification.Abstractions;
using HMS.Implementation.Services;
using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Implementation.Services;
using HotelManagementSystem.Model.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly INotyfService _notyf;

        public ProductController(IProductServices productServices , INotyfService notyf)
        {
            _productServices = productServices;
            _notyf = notyf;
        }

        [HttpGet("get-product")]
        public async Task<IActionResult> Products()
        {
            var product = await _productServices.GetProduct();
            return View(product);
            //return View(new List<ProductDto>());
        }

        [HttpGet("create-product")]
        public async Task<IActionResult> CreateProduct()
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

                _notyf.Success(product.Message, 3);
                return RedirectToAction("Products");
            }
            _notyf.Error(product.Message, 3);
            return RedirectToAction("Products");
        }


        [HttpGet("edit-product/{id}")]
        public async Task<IActionResult> EditProduct([FromRoute] Guid id)
        {
            var product = await _productServices.GetProductAsync(id);

            return View(product.Data);
        }


        [HttpPost("edit-product/{id}")]
        public async Task<IActionResult> EditProduct(UpdateProduct request)
        {

            var product = await _productServices.UpdateProduct(request.Id , request);
            if (product.Success)
            {
                return RedirectToAction("Products");
            }
            return BadRequest();
        }


        

        [HttpGet("delete-product/{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid Id)
        {
            var product = await _productServices.DeleteProductAsync(Id);
            if (product.Success)
            {
                return RedirectToAction("Products", "Product");
            }

            return BadRequest(product);

        }


        [HttpGet("get-all-product-created")]
        public async Task<IActionResult> GetAllProductsAsync()
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
            if (product.Success && product.Data != null)
            {
                return View(product.Data); 
            }
            return RedirectToAction("Products");
        }

        [HttpGet("get-products-by-pagination")]
        public async Task<IActionResult> GetProducts(int pageNumber = 1, int pageSize = 3, decimal price = 0, string searchTerm = null)
        {
            var product = await _productServices.GetAllProductsByPaginationAsync(pageNumber, pageSize, price, searchTerm);
            if (product.Success)
            {
                var paginatedList = new PaginatedList<ProductDto>(product.Data, product.TotalRecords, pageNumber, pageSize);
                return View(paginatedList);
            }
            _notyf.Error(product.Message, 3);
            return View(new PaginatedList<ProductDto>(new List<ProductDto>(), 0, pageNumber, pageSize));
        }


    }
}
