using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using Microsoft.AspNetCore.Mvc;
namespace HotelManagementSystem.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaystackService _paystackService;
        private readonly ApplicationDbContext _dbContext;

        public PaymentController(IPaystackService paystackService, ApplicationDbContext dbContext)
        {
            _paystackService = paystackService;
            _dbContext = dbContext;
        }

        [HttpGet("initiate-payment-form")]
        public IActionResult InitiatePaymentForm(string userId, Guid bookingId)
        {

            var model = new InitializePaymentRequestDto
            {
                UserId = userId,
                BookingId = bookingId
            };

            return View(model);
        }

        [HttpPost("payment/initiate/{userId}/{bookingId}")]
        public async Task<IActionResult> InitiatePayment(InitializePaymentRequestDto requestDto, [FromRoute] string userId, [FromRoute] Guid bookingId)
        {
            if (!ModelState.IsValid)
            {
                requestDto.UserId = userId;
                requestDto.BookingId = bookingId;
                return View("InitiatePaymentForm", requestDto);
            }

            var result = await _paystackService.InitializePaymentAsync(requestDto, userId, bookingId);

            if (result.Success)
            {
                return Redirect(result.Data.AuthorizationUrl);
            }

            ModelState.AddModelError(string.Empty, result.Message);
            return View("InitiatePaymentForm", requestDto);
        }


        [HttpGet("VerifyPayment")]
        public IActionResult VerifyPaymentForm()
        {
            return View();
        }

        [HttpGet("call-back-url")]
        public async Task<IActionResult> PaymentCallback(string reference)
        {
            if (string.IsNullOrEmpty(reference))
            {
                return RedirectToAction("InitiatePayment");
            }

            var result = await _paystackService.VerifyPaymentAsync(reference);

            if (result.Success)
            {
                ViewBag.Reference = reference;
                return View("PaymentSuccess");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Payment verification failed. Please try again.");
                return View("PaymentFailed");
            }
        }

        public IActionResult PaymentSuccess(string reference)
        {
            ViewBag.Reference = reference;
            return View();
        }
    }
}
