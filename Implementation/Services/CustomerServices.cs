using Azure;
using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HMS.Implementation.Services
{
    public class CustomerServices : ICustomerServices
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerServices(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BaseResponse<Guid>> DeleteCustomerAsync(string Id)
        {
            try
            {
                var customer = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == Id);
                if (customer != null)
                {
                    _dbContext.Users.Remove(customer);
                }
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return new BaseResponse<Guid>
                    {
                        Success = true,
                        Message = $"Customer {Id} has Been deleted Succesfully"
                    };
                }
                else
                {
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = $"Failed to delete Customer with {Id}.The room may not exist or there was an error in the deletion process.",
                        Hasherror = true
                    };
                }
            }
            catch
            {
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = $"Failed to delete Customer with {Id}.The room may not exist or there was an error in the deletion process.",
                    Hasherror = true
                };
            }
        }

        public async Task<BaseResponse<CustomerDto>> GetCustomerByIdAsync(string Id)
        {
            var customer = await _dbContext.Users
                .Where(x => x.Id == Id)
                .Select(x => new CustomerDto
                {
                    Address = x.Address,
                    AgeRange = x.AgeRange,
                    Email = x.Email,
                    Gender = x.Gender,
                    Name = x.Name,
                    PhoneNumber = x.PhoneNumber,
                    UserName = x.UserName,
                }).FirstOrDefaultAsync();
            if (customer != null)
            {

                return new BaseResponse<CustomerDto>
                {
                    Success = true,
                    Message = $"Customer with ID {Id} retrieved successfully.",
                    Data = customer
                };
            }
            else
            {
                return new BaseResponse<CustomerDto>
                {
                    Success = false,
                    Message = $"Failed to retrieve customer with ID {Id}.The customer may not exist or there was an error in the retrieval process.",

                    Hasherror = true
                };
            }
        }


        public async Task<List<CustomerDto>> GetCustomer()
        {
            return _dbContext.Users
                //.Include(x => x.Id)
                //.Include(x => x.RoomType)
                .Select(x => new CustomerDto()
                {
                    Id = x.Id,
                    Address = x.Address,
                    AgeRange = x.AgeRange,
                    Email = x.Email,
                    Gender = x.Gender,
                    Name = x.Name,
                    PhoneNumber = x.PhoneNumber,
                    UserName = x.UserName,
                }).ToList();
        }


        public async Task<BaseResponse<CustomerDto>> GetCustomerAsync(string Id)
        {
            var customer = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (customer != null)
            {
                return new BaseResponse<CustomerDto>
                {
                    Message = "",
                    Success = true,
                    Data = new CustomerDto
                    {
                        Address = customer.Address,
                        AgeRange = customer.AgeRange,
                        Email = customer.Email,
                        Gender = customer.Gender,
                        Name = customer.Name,
                        PhoneNumber = customer.PhoneNumber,
                        UserName = customer.UserName,

                    }

                };
            }
            return new BaseResponse<CustomerDto>
            {
                Success = false,
                Message = "",
            };
        }


        public async Task<BaseResponse<IList<CustomerDto>>> GetAllCustomerCreatedAsync()
        {
            var customer = await _dbContext.Users
               .Select(x => new CustomerDto
               {
                   Address = x.Address,
                   AgeRange = x.AgeRange,
                   Email = x.Email,
                   Gender = x.Gender,
                   Name = x.Name,
                   PhoneNumber = x.PhoneNumber,
                   UserName = x.UserName,
               }).ToListAsync();
            if (customer != null)
            {
                return new BaseResponse<IList<CustomerDto>>
                {
                    Success = true,
                    Message = $"Customer  retrieved successfully.",
                    Data = customer
                };
            }
            else
            {
                return new BaseResponse<IList<CustomerDto>>
                {
                    Success = false,
                    Message = $"Failed to retrieve customer there was an error in the retrieval process.",
                    // Hasherror = true
                };
            }
        }

        public async Task<BaseResponse<CustomerDto>> UpdateCustomer(string Id, UpdateCustomer request)
        {
            try
            {
                var customer = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == Id);
                if (customer == null)
                {
                    return new BaseResponse<CustomerDto>
                    {
                        Success = false,
                        Message = $"Failed to Update customer {request.UserName} ,there was an error in the updating process.",
                        Hasherror = true
                    };
                }

                request.Id = request.Id;
                request.Address = request.Address;
                request.PhoneNumber = request.PhoneNumber;
                request.UserName = request.UserName;
                request.DateOfBirth = request.DateOfBirth;
                request.Email = request.Email;
                request.Gender = request.Gender;
                request.Password = request.Password;
                _dbContext.Users.Update(customer);
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return new BaseResponse<CustomerDto>
                    {
                        Success = true,
                        Message = $"Customer with ID {Id} Updated successfully."
                    };
                }
                else
                {
                    return new BaseResponse<CustomerDto>
                    {
                        Success = false,
                        Message = $"Failed to Update customer {request.UserName} ,there was an error in the updating process.",
                        Hasherror = true
                    };
                }
            }
            catch (Exception ex)
            {

                return new BaseResponse<CustomerDto>
                {
                    Success = false,
                    Message = $"Failed to Update customer {request.UserName} ,there was an error in the updating process.",
                    Hasherror = true
                };
            }

        }


    }

}
