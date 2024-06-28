﻿using HotelManagementSystem.Dto;
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

        public CustomerStatusServices(ApplicationDbContext dbContext, IBookingServices bookingServices)
        {
            _dbContext = dbContext;
            _bookingServices = bookingServices;
        }



        public async Task<BaseResponse<Guid>> CheckIn(Guid customerId , Guid bookingId)
        {
            var customer = await _dbContext.Customers.FindAsync(customerId);
            if (customer == null)
            {
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "Booking not found."
                };
            }
          

            var customerStatus = new CustomerStatus
            {
               // BookingId = customer.Id,
                CustomerName = customer.Name,
                CheckInDate = DateTime.Now,
               
            };

            _dbContext.CustomerStatuses.Add(customerStatus);
            await _dbContext.SaveChangesAsync();

            //booking.Rooms.IsAvailable = false;
            //_dbContext.Rooms.Update(booking.Rooms);
            //await _dbContext.SaveChangesAsync();

            return new BaseResponse<Guid>
            {
                Success = true,
                Message = "Check-in successful.",
                Data = customerId
            };
        }

        

        //public async Task<BaseResponse<Guid>> CheckOut(Guid customerId, Guid bookingId)
        //{
        //    var customer = await _dbContext.Customers.FindAsync(customerId);
        //    if (customer == null)
        //    {
        //        return new BaseResponse<Guid>
        //        {
        //            Success = false,
        //            Message = "Booking not found."
        //        };
        //    }

        //    var booking = await _dbContext.Bookings.FindAsync(bookingId);
        //    {
        //        if (booking == null)
        //        {
        //            return new BaseResponse<Guid>
        //            {
        //                Success = false,
        //                Message = "Booking not found."
        //            };
        //        }
        //    }

        //    customer..CheckOutDate = DateTime.Now;
        //    _dbContext.CustomerStatuses.Update(customerStatus);
        //    await _dbContext.SaveChangesAsync();

        //    //booking.Rooms.IsAvailable = true;
        //    //_dbContext.Rooms.Update(booking.Rooms);
        //    //await _dbContext.SaveChangesAsync();

        //    return new BaseResponse<Guid>
        //    {
        //        Success = true,
        //        Message = "Check-out successful.",
        //        Data = bookingId
        //    };
        //}

        public async Task<BaseResponse<Guid>> CheckOut(Guid customerId, Guid bookingId)
        {
            var customer = await _dbContext.Customers.FindAsync(customerId);
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

            var customerStatus = await _dbContext.CustomerStatuses
                .FirstOrDefaultAsync(cs => cs.CustomerId == customerId && cs.BookingId == bookingId);

            if (customerStatus == null)
            {
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "Customer status not found."
                };
            }

            customerStatus.CheckOutDate = DateTime.Now;
            _dbContext.CustomerStatuses.Update(customerStatus);

            ////Set room availability to true
            //var room = await _dbContext.Rooms.FindAsync(roomId);
            //if (room != null)
            //{
            //    room.IsAvailable = true;
            //    _dbContext.Rooms.Update(room);
            //}

            await _dbContext.SaveChangesAsync();

            return new BaseResponse<Guid>
            {
                Success = true,
                Message = "Check-out successful.",
                Data = customerStatus.Id
            };
        }

        public async Task<List<CustomerStatusDto>> GetCustomerStatus()
        {
            return _dbContext.CustomerStatuses
                .Select(x => new CustomerStatusDto()
                {

                    CheckInDate = x.CheckInDate,
                    //IsPaid = x.IsPaid,
                    CheckOutDate = x.CheckOutDate,
                    CustomerName = x.CustomerName

                }).ToList();
        }


        public List<SelectCustomerDto> GetCustomerSelect()
        {
            var customers = _dbContext.Customers.ToList();
            var result = new List<SelectCustomerDto>();

            if (customers.Count > 0)
            {
                result = customers.Select(x => new SelectCustomerDto()
                {
                    Id = x.Id,
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

        public List<SelectBookingDto> GetBookingSelect()
        {
            var bookings = _dbContext.Bookings.ToList();
            var result = new List<SelectBookingDto>();

            if (bookings.Count > 0)
            {
                result = bookings.Select(x => new SelectBookingDto()
                {
                    Id = x.Id,
                     Email = x.Email
                }).ToList();
            }

            return result;
        }
    }
}