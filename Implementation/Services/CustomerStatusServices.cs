using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using HotelManagementSystem.Model.Entity.Enum;
using HotelManagementSystem.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Implementation.Services
{
    public class CustomerStatusServices : ICustomerStatusServices
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBookingServices _bookingServices;
        private readonly IUserServices _userServices;

        public CustomerStatusServices(ApplicationDbContext dbContext, IBookingServices bookingServices,IUserServices userServices)
        {
            _dbContext = dbContext;
            _bookingServices = bookingServices;
            _userServices = userServices;
        }

        public async Task<BaseResponse<Guid>> CheckIn(string customerId, Guid bookingId)
        {
            try
            {
                var customer = await _dbContext.Users.FindAsync(customerId);
                if (customer == null)
                {
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "Customer not found."
                    };
                }

                //var booking = await _dbContext.Bookings.FindAsync(bookingId);
                //if (booking == null)
                //{
                //    return new BaseResponse<Guid>
                //    {
                //        Success = false,
                //        Message = "Booking not found."
                //    };
                //}

                var customerStatus = new CustomerStatus
                {
                    BookingId = bookingId,
                    CustomerId = customerId,
                    CustomerName = customer.Name,
                    CheckInDate = DateTime.Now,
                };

                _dbContext.CustomerStatuses.Add(customerStatus);
                await _dbContext.SaveChangesAsync();

                // Assume Rooms is a related entity in Booking and you want to update its availability
                //booking.Rooms.IsAvailable = false;
                //_dbContext.Rooms.Update(booking.Rooms);
                //await _dbContext.SaveChangesAsync();

                return new BaseResponse<Guid>
                {
                    Success = true,
                    Message = "Check-in successful.",
                    Data = customerStatus.BookingId
                };

            }


            catch (Exception ex)
            {
                return new BaseResponse<Guid>
                {
                    Success = true,
                    Message = "Check-in failed.",

                };
            }

        }

        public async Task<BaseResponse<Guid>> CheckOut(Guid customerId)
        {
            try
            {
                var customerStatus = await _dbContext.CustomerStatuses.FirstOrDefaultAsync(x => x.Id == customerId);
                if (customerStatus == null)
                {
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "Customer not found."
                    };
                }
                customerStatus.CheckOutDate = DateTime.Now;

                _dbContext.CustomerStatuses.Update(customerStatus);
                await _dbContext.SaveChangesAsync();
                return new BaseResponse<Guid>
                {
                    Success = true,
                    Message = "Check-out successful.",
                   Data = customerStatus.Id
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<Guid>
                {
                    Success = true,
                    Message = "Check-out failed.",
                };
            }
        }
          
        public async Task<List<CustomerStatusDto>> GetCustomerStatus()
        {
            return _dbContext.CustomerStatuses
                .Select(x => new CustomerStatusDto()
                {
                    Id = x.Id,
                    CheckInDate = x.CheckInDate,
                    CheckOutDate = x.CheckOutDate,
                    CustomerName = x.CustomerName,
                    CustomerId = x.Id


                }).ToList();
        }

        public List<SelectCustomerDto> GetCustomerSelect()
        {
            var customers = _dbContext.Users.ToList();
            var result = new List<SelectCustomerDto>();

            if (customers.Count > 0)
            {
                result = customers.Select(x => new SelectCustomerDto()
                {
                    Id = Guid.Parse(x.Id),
                    Name = x.Name,
                }).ToList();
            }

            return result;
        }

        public List<SelectProductDto> GetProductSelect()
        {
            var products = _dbContext.Products.ToList();
            var result = new List<SelectProductDto>();

            if (products.Count > 0)
            {
                result = products.Select(x => new SelectProductDto()
                {
                    Id = x.Id,
                    ProductName = x.Name,
                }).ToList();
            }

            return result;
        }
        public List<SelectCustomerCheckedInDto> GetSelectCustomerCheckedIn()
        {
            var customerStatus = _dbContext.CustomerStatuses.ToList();
            var result = new List<SelectCustomerCheckedInDto>();

            if (customerStatus.Count > 0)
            {
                result = customerStatus.Select(x => new SelectCustomerCheckedInDto()
                {
                    Id = x.Id,
                    Name = x.CustomerName,
                }).ToList();
            }

            return result;
        }
    }
}
