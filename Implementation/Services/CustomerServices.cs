using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using Microsoft.EntityFrameworkCore;
namespace HMS.Implementation.Services
{
    public class CustomerServices : ICustomerServices
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CustomerServices> _logger;

        public CustomerServices(ApplicationDbContext dbContext, ILogger<CustomerServices> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<BaseResponse<Guid>> DeleteCustomerAsync(string Id)
        {
            _logger.LogInformation("DeleteCustomerAsync method called with Id: {Id}", Id);
            try
            {
                var customer = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == Id);
                if (customer != null)
                {
                    _dbContext.Users.Remove(customer);
                }
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    _logger.LogInformation("Customer with Id: {Id} deleted successfully.", Id);
                    return new BaseResponse<Guid>
                    {
                        Success = true,
                        Message = $"Customer {Id} has Been deleted successfully"
                    };
                }
                else
                {
                    _logger.LogWarning("Failed to delete customer with Id: {Id}", Id);
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = $"Failed to delete Customer with {Id}. The customer may not exist or there was an error in the deletion process.",
                        Hasherror = true
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the customer with Id: {Id}", Id);
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = $"Failed to delete Customer with {Id}. The customer may not exist or there was an error in the deletion process.",
                    Hasherror = true
                };
            }
        }

        public async Task<BaseResponse<CustomerDto>> GetCustomerByIdAsync(string Id)
        {
            _logger.LogInformation("GetCustomerByIdAsync method called with Id: {Id}", Id);
            var customer = await _dbContext.Users
                .Where(x => x.Id == Id)
                .Select(x => new CustomerDto
                {
                    Address = x.Address,
                    AgeRange = x.AgeRange,
                    Email = x.Email,
                    Gender = x.Gender,
                    Name = x.FullName,
                    PhoneNumber = x.PhoneNumber,
                    UserName = x.UserName,
                }).FirstOrDefaultAsync();
            if (customer != null)
            {
                _logger.LogInformation("Customer retrieved successfully for Id: {Id}", Id);
                return new BaseResponse<CustomerDto>
                {
                    Success = true,
                    Message = $"Customer with ID {Id} retrieved successfully.",
                    Data = customer
                };
            }
            else
            {
                _logger.LogWarning("Failed to retrieve customer with Id: {Id}", Id);
                return new BaseResponse<CustomerDto>
                {
                    Success = false,
                    Message = $"Failed to retrieve customer with ID {Id}. The customer may not exist or there was an error in the retrieval process.",
                    Hasherror = true
                };
            }
        }

        public async Task<List<CustomerDto>> GetCustomer()
        {
            _logger.LogInformation("GetCustomer method called.");
            var customers = await _dbContext.Users
                .Select(x => new CustomerDto
                {
                    Id = x.Id,
                    Address = x.Address,
                    AgeRange = x.AgeRange,
                    Email = x.Email,
                    Gender = x.Gender,
                    Name = x.FullName,
                    PhoneNumber = x.PhoneNumber,
                    UserName = x.UserName,
                }).ToListAsync();
            _logger.LogInformation("Retrieved {Count} customers.", customers.Count);
            return customers;
        }

        public async Task<BaseResponse<CustomerDto>> GetCustomerAsync(string Id)
        {
            _logger.LogInformation("GetCustomerAsync method called with Id: {Id}", Id);
            var customer = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (customer != null)
            {
                _logger.LogInformation("Customer retrieved successfully for Id: {Id}", Id);
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
                        Name = customer.FullName,
                        PhoneNumber = customer.PhoneNumber,
                        UserName = customer.UserName,
                    }
                };
            }
            _logger.LogWarning("Customer not found for Id: {Id}", Id);
            return new BaseResponse<CustomerDto>
            {
                Success = false,
                Message = "",
            };
        }

        public async Task<BaseResponse<IList<CustomerDto>>> GetAllCustomerCreatedAsync()
        {
            _logger.LogInformation("GetAllCustomerCreatedAsync method called.");
            var customers = await _dbContext.Users
               .Select(x => new CustomerDto
               {
                   Address = x.Address,
                   AgeRange = x.AgeRange,
                   Email = x.Email,
                   Gender = x.Gender,
                   Name = x.FullName,
                   PhoneNumber = x.PhoneNumber,
                   UserName = x.UserName,
               }).ToListAsync();
            if (customers != null)
            {
                _logger.LogInformation("Retrieved {Count} customers.", customers.Count);
                return new BaseResponse<IList<CustomerDto>>
                {
                    Success = true,
                    Message = "Customers retrieved successfully.",
                    Data = customers
                };
            }
            else
            {
                _logger.LogWarning("Failed to retrieve customers.");
                return new BaseResponse<IList<CustomerDto>>
                {
                    Success = false,
                    Message = "Failed to retrieve customers. There was an error in the retrieval process.",
                };
            }
        }

        public async Task<BaseResponse<CustomerDto>> UpdateCustomer(string Id, UpdateCustomer request)
        {
            _logger.LogInformation("UpdateCustomer method called with Id: {Id}", Id);
            try
            {
                var customer = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == Id);
                if (customer == null)
                {
                    _logger.LogWarning("Customer not found for Id: {Id}", Id);
                    return new BaseResponse<CustomerDto>
                    {
                        Success = false,
                        Message = $"Failed to update customer {request.UserName}. There was an error in the updating process.",
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
                _dbContext.Users.Update(customer);
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    _logger.LogInformation("Customer with Id: {Id} updated successfully.", Id);
                    return new BaseResponse<CustomerDto>
                    {
                        Success = true,
                        Message = $"Customer with ID {Id} updated successfully."
                    };
                }
                else
                {
                    _logger.LogWarning("Failed to update customer with Id: {Id}", Id);
                    return new BaseResponse<CustomerDto>
                    {
                        Success = false,
                        Message = $"Failed to update customer {request.UserName}. There was an error in the updating process.",
                        Hasherror = true
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the customer with Id: {Id}", Id);
                return new BaseResponse<CustomerDto>
                {
                    Success = false,
                    Message = $"Failed to update customer {request.UserName}. There was an error in the updating process.",
                    Hasherror = true
                };
            }
        }
    }
}
