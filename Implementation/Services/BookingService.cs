using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Implementation.Services
{
    public class BookingService : IBookingServices
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICustomerServices _customerServices;
        private readonly IRoomService _roomService;

        public BookingService(ApplicationDbContext dbContext, ICustomerServices customerServices, IRoomService roomService)
        {
            _dbContext = dbContext;
            _customerServices = customerServices;
            _roomService = roomService;
        }


        public async Task<BaseResponse<Guid>> CreateBooking(CreateBooking request, Guid Id)
        {
            if (request != null)
            {

                // Check if the customer is registered

                //var customer = await _customerServices.GetCustomerByIdAsync(Id);
                //if (customer.Data == null)
                //{
                //    return new BaseResponse<Guid>
                //    {
                //        Success = false,
                //        Message = "Booking Failed. Customer is not registered.",
                //        //Hasherror = true
                //    };
                //}


                foreach (var room in request.Rooms)
                {
                    var roomDetails = await _roomService.GetRoomsByIdAsync(room.RoomId);
                    if (roomDetails == null)
                    {
                        return new BaseResponse<Guid>
                        {
                            Success = false,
                            Message = "Booking Failed. The room is not available.",
                            Hasherror = true
                        };
                    }
                }

                var existingBooking = _dbContext.Bookings.FirstOrDefault(x =>
                   //x.CheckIn == request.CheckIn &&
                   //x.Checkout == request.Checkout &&
                   x.Status == request.Status);

                if (existingBooking != null)
                {
                    // Booking already exists
                    return new BaseResponse<Guid>
                    {
                        Success = true,
                        Message = "Booking already exists.",
                        Hasherror = true
                    };

                }

                var booking = new Booking()
                {
                    //CheckIn = request.CheckIn,
                    // Checkout = request.Checkout,
                    Status = request.Status,
                    RoomId = request.RoomId,
                    TotalCost = request.TotalCost,

                };
                _dbContext.Bookings.Add(booking);



                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return new BaseResponse<Guid>
                    {
                        Success = true,
                        Message = "Your booking  has been successful"
                    };
                }
                else
                {
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "Booking Failed, unable to complete your booking request.",
                        Hasherror = true
                    };
                }
            }
            else
            {
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "Booking Failed, unable to complete your booking request.",
                    Hasherror = true
                };
            }
        }


        public async Task<List<BookingDto>> GetBooking()
        {
            return _dbContext.Bookings
                //.Include(x => x.Id)
                //.Include(x => x.RoomType)
                .Select(x => new BookingDto()
                {
                    // Id = x.Id,
                    CheckIn = x.CheckIn,
                    Checkout = x.Checkout,
                    Status = x.Status,
                    Rooms = x.Rooms


                }).ToList();
        }



        public async Task<BaseResponse<Guid>> DeleteBookingAsync(Guid Id)
        {
            var booking = _dbContext.Bookings.FirstOrDefault();
            if (booking != null)
            {
                _dbContext.Bookings.Remove(booking);
            }
            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return new BaseResponse<Guid>
                {
                    Success = true,
                    Message = $"Booking {Id} Has been Deleted Succesfully"
                };
            }
            else
            {
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = $"Delete Failed unable to process the deletion of  booking {Id} at this time",
                    Hasherror = true
                };
            }
        }



        //public List<SelectRoomDto> GetRoomSelect()
        //{
        //    var roomName = _dbContext.Rooms.ToList();

        //    var result = new List<SelectRoomDto>();

        //    if (roomName.Count > 0)
        //    {
        //        result = roomName.Select(x => new SelectRoomDto()
        //        {
        //            Id = x.Id,
        //            RoomName = x.RoomName,
        //        }).ToList();
        //    } 

        //    return result;
        //}

        public List<SelectRoomDto> GetRoomSelect()
        {
            var roomName = _dbContext.Rooms.ToList();
            var result = new List<SelectRoomDto>();

            if (roomName.Count > 0)
            {
                result = roomName.Select(x => new SelectRoomDto()
                {
                    Id = x.Id,
                    RoomName = x.RoomName,
                }).ToList();
            }

            return result;
        }


        public async Task<BaseResponse<BookingDto>> GetBookingAsync(Guid Id)
        {
            var booking = await _dbContext.Bookings.FirstOrDefaultAsync(x => x.Id == Id);
            if (booking != null)
            {
                return new BaseResponse<BookingDto>
                {
                    Message = "",
                    Success = true,
                    Data = new BookingDto
                    {
                        TotalCost = booking.TotalCost,
                        CheckIn = booking.CheckIn,
                        Status = booking.Status,
                        Checkout = booking.Checkout
                    }

                };
            }
            return new BaseResponse<BookingDto>
            {
                Success = false,
                Message = "",
            };
        }


        public async Task<BaseResponse<IList<BookingDto>>> GetBookingByIdAsync(Guid Id)
        {
            var bookings = await _dbContext.Bookings
             .Where(x => x.Id == Id)
             .Select(x => new BookingDto()
             {

                 CheckIn = x.CheckIn,
                 Checkout = x.Checkout,
                 Status = x.Status,
                 TotalCost = x.TotalCost,
                 //  Id = x.RoomId,
             }).ToListAsync();
            if (bookings != null)
            {
                return new BaseResponse<IList<BookingDto>>
                {
                    Success = true,
                    Message = "Bookings Retrieved Succesfully",
                    Data = bookings
                };
            }
            else
            {
                return new BaseResponse<IList<BookingDto>>
                {
                    Success = false,
                    Message = "Booking Retrieved Failed",
                    Hasherror = true
                };
            }
        }


        public async Task<BaseResponse<IList<BookingDto>>> GetAllBookingsAsync()
        {
            var booking = await _dbContext.Bookings
            .Select(x => new BookingDto()
            {

                CheckIn = x.CheckIn,
                Checkout = x.Checkout,
                Status = x.Status,
                TotalCost = x.TotalCost,
                // RoomId = x.RoomId,
            }).ToListAsync();
            if (booking != null)
            {
                return new BaseResponse<IList<BookingDto>>
                {
                    Success = true,
                    Message = "Bookings Retrieved Succesfully",
                    Data = booking
                };
            }
            else
            {
                return new BaseResponse<IList<BookingDto>>
                {
                    Success = false,
                    Message = "Bookings Retrieval Failed",
                    Data = booking,
                    Hasherror = true
                };
            }
        }

        public async Task<BaseResponse<BookingDto>> UpdateBooking(Guid Id, UpdateBooking request)
        {
            var booking = _dbContext.Bookings.FirstOrDefault();
            if (booking == null)
            {
                return new BaseResponse<BookingDto>
                {
                    Success = true,
                    Message = $"Booking {Id} Updated Succesfully"
                };
            }
            //booking.CheckIn = request.CheckIn;
            //booking.Checkout = request.Checkout;
            booking.Status = request.Status;
            booking.TotalCost = request.TotalCost;
            _dbContext.Bookings.Update(booking);

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return new BaseResponse<BookingDto>
                {
                    Success = true,
                    Message = $"Booking {Id} Updated Succesfully"
                };
            }
            else
            {
                return new BaseResponse<BookingDto>
                {
                    Success = false,
                    Message = $"Booking {Id} Update Failed",
                    Hasherror = false
                };
            }
        }


    }
}
