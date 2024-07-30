using Azure.Core;
using HMS.Implementation.Services;
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

        public BookingService(ApplicationDbContext dbContext, ICustomerServices customerServices)
        {
            _dbContext = dbContext;
            _customerServices = customerServices;
        }


        public async Task<BaseResponse<Guid>> CreateBooking(CreateBooking request)
        {
            try
            {
                // Retrieve the product from the database
                var room = await _dbContext.Rooms.FindAsync(request.RoomId);
                if (room == null)
                {
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "Room not found",
                        Hasherror = true
                    };
                }


                //foreach (var room in request.Rooms)
                //{
                //    var roomDetails = await _roomService.GetRoomByIdAsync(room.RoomId);
                //    if (roomDetails == null)
                //    {
                //        return new BaseResponse<Guid>
                //        {
                //            Success = false,
                //            Message = "Booking Failed. The room is not available.",
                //            Hasherror = true
                //        };
                //    }
                //}



                var booking = new Booking()
                {
                    RoomId = request.RoomId,
                    CustomerId = request.CustomerId,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    TotalCost = room.RoomRate,
                    // Rooms = request.Rooms.Select(r => new Room { RoomId = r.RoomId }).ToList()
                };

                _dbContext.Bookings.Add(booking);

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return new BaseResponse<Guid>
                    {
                        Success = true,
                        Message = "Your booking has been successful",
                        Data = booking.Id
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
            catch (Exception ex)
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
                    Id = x.Id,
                    RoomId = x.RoomId,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    TotalCost = x.TotalCost,
                    RoomName = x.Rooms.RoomName,

                }).ToList();
        }


        public async Task<BaseResponse<Guid>> DeleteBookingAsync(Guid Id)
        {
            try
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
            catch
            {

                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = $"Delete Failed unable to process the deletion of  booking {Id} at this time",
                    Hasherror = true
                };
            }


        }

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
                        RoomId = booking.RoomId,
                        Email = booking.Email,
                        PhoneNumber = booking.PhoneNumber,
                        RoomName = booking.Rooms.RoomName,
                    }

                };
            }
            return new BaseResponse<BookingDto>
            {
                Success = false,
                Message = "",
            };
        }



        public async Task<BaseResponse<BookingDto>> GetBookingByIdAsync(Guid Id)
        {
            var bookings = await _dbContext.Bookings
             .Where(x => x.Id == Id)
             .Select(x => new BookingDto()
             {

                 //CheckIn = x.CheckIn,
                 //Checkout = x.Checkout,
                 Id = x.Id,
                 Rooms = x.Rooms,
                 RoomId = x.RoomId,
                 Email = x.Email,
                 PhoneNumber = x.PhoneNumber,
                 TotalCost = x.TotalCost,
             }).FirstOrDefaultAsync();
            if (bookings != null)
            {
                return new BaseResponse<BookingDto>
                {
                    Success = true,
                    Message = "Bookings Retrieved Succesfully",
                    Data = bookings
                };
            }
            else
            {
                return new BaseResponse<BookingDto>
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

                TotalCost = x.TotalCost,
                RoomId = x.RoomId,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
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
            try
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
                var room = await _dbContext.Rooms.FindAsync(request.RoomId);
                {
                    if (room == null)
                    {
                        return new BaseResponse<BookingDto>
                        {
                            Success = true,
                            Message = "Room not Found"
                        };
                    }

                }
                booking.TotalCost = room.RoomRate;
                booking.RoomId = request.RoomId;
                booking.PhoneNumber = request.PhoneNumber;
                booking.Email = request.Email;
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
            catch (Exception ex)
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
