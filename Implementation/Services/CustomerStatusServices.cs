using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using HotelManagementSystem.Model.Entity.Enum;
using HotelManagementSystem.Models.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace HotelManagementSystem.Implementation.Services
{
    public class CustomerStatusServices : ICustomerStatusServices
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBookingServices _bookingServices;
        private readonly IUserServices _userServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<CustomerStatusServices> _logger;

        public CustomerStatusServices(ApplicationDbContext dbContext, IBookingServices bookingServices,
         IHttpContextAccessor httpContextAccessor,
         UserManager<User> userManager, IUserServices userServices, ILogger<CustomerStatusServices> logger)
        {
            _dbContext = dbContext;
            _bookingServices = bookingServices;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _userServices = userServices;
            _logger = logger;
        }

        public async Task<BaseResponse<Guid>> CheckIn(string customerId, Guid bookingId)
        {
            _logger.LogInformation("CheckIn called with customerId: {customerId}, bookingId: {bookingId}", customerId, bookingId);
            try
            {
                var customer = await _dbContext.Users.FindAsync(customerId);
                if (customer == null)
                {
                    _logger.LogWarning("Customer not found with Id: {customerId}", customerId);
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "Customer not found."
                    };
                }
                var booking = await _dbContext.Bookings.FindAsync(bookingId);
                if (booking == null)
                {
                    _logger.LogWarning("Booking not found .");
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "Booking not found ."
                    };
                }

                var room = await _dbContext.Rooms.FindAsync(booking.RoomId);
                if (room == null)
                {
                    _logger.LogWarning("Room not found for BookingId: {bookingId}", bookingId);
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "Room not found for this booking."
                    };
                }

                var userPrincipal = _httpContextAccessor.HttpContext?.User;
                if (userPrincipal == null)
                {
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "User not authenticated"
                    };
                }

                var user = await _userManager.GetUserAsync(userPrincipal);
                if (user == null)
                {
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                var customerStatus = new CustomerStatus
                {
                    BookingId = bookingId,
                    CustomerId = customerId,
                    CustomerName = customer.FullName,
                    CheckInDate = DateTime.Now,
                    CreatedBy = user.UserName
                };
                _dbContext.CustomerStatuses.Add(customerStatus);
                room.RoomStatus = RoomStatus.CheckedIn;
                _dbContext.Rooms.Update(room);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Check-in successful for customerId: {customerId}", customerId);
                return new BaseResponse<Guid>
                {
                    Success = true,
                    Message = "Check-in successful.",
                    Data = customerStatus.BookingId
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred during check-in for customerId: {customerId}", customerId);
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "Check-in failed.",
                };
            }
        }


        public async Task<BaseResponse<Guid>> CheckOut(string customerId)
        {
            _logger.LogInformation("CheckOut called with customerId: {customerId}", customerId);
            try
            {
                var customerStatus = await _dbContext.CustomerStatuses
            .FirstOrDefaultAsync(x => x.CustomerId == customerId);

                if (customerStatus == null)
                {
                    _logger.LogWarning("CustomerStatus not found with Id: {customerId}", customerId);
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "Customer not found."
                    };
                }

                var booking = await _dbContext.Bookings.FindAsync(customerStatus.BookingId);
                if (booking == null)
                {
                    _logger.LogWarning("Booking not found for CustomerStatus Id: {customerStatusId}", customerStatus.Id);
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "Booking not found."
                    };
                }
                customerStatus.CheckOutDate = DateTime.Now;
                var room = await _dbContext.Rooms.FindAsync(booking.RoomId);
                if (room == null)
                {
                    _logger.LogWarning("Room not found for BookingId: {bookingId}", booking.Id);
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "Room not found for this booking."
                    };
                }
                room.RoomStatus = RoomStatus.Pending;
                room.Availability = RoomAvailability.Available;
                _dbContext.CustomerStatuses.Update(customerStatus);
                _dbContext.Rooms.Update(room);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Check-out successful for customerId: {customerId} and room status set to Available.", customerId);
                return new BaseResponse<Guid>
                {
                    Success = true,
                    Message = "Check-out successful. Room is now available.",
                    Data = customerStatus.Id
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred during check-out for customerId: {customerId}", customerId);
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "Check-out failed.",
                };
            }
        }



        public async Task<List<CustomerStatusDto>> GetCustomerStatus()
        {
            _logger.LogInformation("GetCustomerStatus called");
            var userPrincipal = _httpContextAccessor.HttpContext?.User;
            if (userPrincipal == null)
            {
                return new List<CustomerStatusDto> { };
            }

            var user = await _userManager.GetUserAsync(userPrincipal);
            if (user == null)
            {
                return new List<CustomerStatusDto> { };
            }
            return await _dbContext.CustomerStatuses
                .Where(x => x.CreatedBy == user.UserName)
                .Select(x => new CustomerStatusDto()
                {
                    Id = x.Id,
                    CheckInDate = x.CheckInDate,
                    CheckOutDate = x.CheckOutDate,
                    CustomerName = x.CustomerName,
                    CustomerId = x.CustomerId
                }).ToListAsync();
        }

        public List<SelectCustomerDto> GetCustomerSelect()
        {
            _logger.LogInformation("GetCustomerSelect called");
            var customers = _dbContext.Users.ToList();
            var result = new List<SelectCustomerDto>();

            if (customers.Count > 0)
            {
                result = customers.Select(x => new SelectCustomerDto()
                {
                    Id = Guid.Parse(x.Id),
                    Name = x.FullName,
                }).ToList();
            }

            return result;
        }

        public List<SelectCustomerCheckedInDto> GetSelectCustomerCheckedIn()
        {
            _logger.LogInformation("GetSelectCustomerCheckedIn called");
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
