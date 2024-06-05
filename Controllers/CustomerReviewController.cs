using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Implementation.Services;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Controllers
{
    public class CustomerReviewController : Controller
    {
        private readonly ICustomerReviewService _customerReviewService;

        public CustomerReviewController(ICustomerReviewService customerReviewService)
        {
            _customerReviewService = customerReviewService;
        }


        [HttpGet("get-review")]
        public async Task<IActionResult> Index()
        {
            var review = await _customerReviewService.GetReview();
            return View(review);
        }


        public async Task<IActionResult> Create()
        {
            var review = await _customerReviewService.GetAllReviewAsync();
            if (review.Success)
            {

                return View();
            }
            return BadRequest();
        }

        [HttpPost("create-review")]
        public async Task<IActionResult> CreateReview(CreateReview request, Guid Id)
        {
            var review = await _customerReviewService.CreateReview(request, Id);
            if (review.Success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest(review);
            }

        }
       
        [HttpGet("get-all-review-created")]
        public async Task<IActionResult> GetAllReviewAsync()
        {
            var review = await _customerReviewService.GetAllReviewAsync();
            if (review.Success == false)
            {
                return View(review);
            }
            else
            {
                return BadRequest(review);
            }


        }

    }
}
