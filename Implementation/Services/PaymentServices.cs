using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using HotelManagementSystem.Model.Entity.Enum;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Implementation.Services
{
    public class PaymentServices : IPaymentServices
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBookingServices _bookingServices;

        public PaymentServices(ApplicationDbContext dbContext, IBookingServices bookingServices)
        {
            _dbContext = dbContext;
            _bookingServices = bookingServices;
        }


        public async Task<BaseResponse<Guid>> CreatePayment(CreatePayment request)
        {
            try
            {
                var payment = new Payment
                {
                    BookingId = request.BookingId,
                    PaymentMethod = request.PaymentMethod,
                    PaymentDate = request.PaymentDate,
                    Amount = request.Amount,
                    Balance = request.Balance,
                    PaymentStatus = request.PaymentStatus
                };

                _dbContext.payments.Add(payment);
                await _dbContext.SaveChangesAsync();
                return new BaseResponse<Guid>
                {
                    Success = true,
                    Message = $"payment Successfully"

                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Guid>
                {
                    Success = true,
                    Message = $"payment Failed"

                };
            }
        }

        public async Task<PaymentDto> GetPaymentById(Guid paymentId)
        {
            var payment = await _dbContext.payments.FindAsync(paymentId);
            if (payment == null)
            {
                return null;
            }

            return new PaymentDto
            {
                PaymentId = payment.Id,
                BookingId = payment.BookingId,
                PaymentMethod = payment.PaymentMethod,
                PaymentDate = payment.PaymentDate,
                Amount = payment.Amount,
                Balance = payment.Balance,
                PaymentStatus = payment.PaymentStatus
            };
        }

        public async Task<List<PaymentDto>> GetPayment()
        {
            return _dbContext.payments
                .Select(x => new PaymentDto()
                {
                    Amount = x.Amount,
                    Balance = x.Balance,
                    BookingId = x.BookingId,
                    PaymentDate = x.PaymentDate,
                    PaymentId = x.Id,
                    PaymentMethod = x.PaymentMethod,
                    PaymentStatus = x.PaymentStatus


                }).ToList();
        }

        public async Task<BaseResponse<IList<PaymentDto>>> GetAllPaymentAsync()
        {
            var payment = await _dbContext.payments
             .Select(x => new PaymentDto()
             {
                 Amount = x.Amount,
                 Balance = x.Balance,
                 BookingId = x.BookingId,
                 PaymentDate = x.PaymentDate,
                 PaymentId = x.Id,
                 PaymentMethod = x.PaymentMethod,
                 PaymentStatus = x.PaymentStatus
             }).ToListAsync();


            return new BaseResponse<IList<PaymentDto>>
            {
                Success = true,
                Message = "Payment Succesfully Retrieved",
                Data = payment
            };
        }
    }
}
