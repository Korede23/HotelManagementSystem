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

        public CustomerServices(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BaseResponse<Guid>> CreateCustomer(CreateCustomer request)
        {
            try
            {
                if (request != null)
                {
                    var existingCustomer = _dbContext.Customers.FirstOrDefault(x =>
                    // x.Id == request.Id &&
                    x.UserName == request.UserName &&
                    x.Email == request.Email);

                    if (existingCustomer != null)
                    {
                        return new BaseResponse<Guid>
                        {
                            Success = true,
                            Message = $"Customer {request.UserName} already exists.",
                            Hasherror = true
                        };
                    }

                    var customer = new Customer
                    {
                        //  Id = request.Id,
                        UserName = request.UserName,
                        Email = request.Email,
                        Address = request.Address,
                        Age = request.Age,
                        Gender = request.Gender,
                        Name = request.Name,
                        Password = request.Password,
                        PhoneNumber = request.PhoneNumber,
                    };
                    _dbContext.Customers.Add(customer);
                }
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return new BaseResponse<Guid>
                    {
                        Success = true,
                        Message = $"Registration Successful, Congratulations {request.UserName}",
                    };
                }
                else
                {
                    return new BaseResponse<Guid>
                    {
                        Message = "Failed"
                    };
                }


            }
            catch (Exception ex)
            {
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = $"Registration Failed, Unable to register {request.UserName}",
                    Hasherror = true

                };
            }
        }


        public async Task<BaseResponse<Guid>> DeleteCustomerAsync(Guid Id)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync();
            if (customer != null)
            {
                _dbContext.Customers.Remove(customer);
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



        public async Task<BaseResponse<IList<CustomerDto>>> GetCustomerByIdAsync(Guid Id)
        {
            var customer = await _dbContext.Customers
                .Where(x => x.Id == Id)
                .Select(x => new CustomerDto
                {
                    Address = x.Address,
                    Age = x.Age,
                    Email = x.Email,
                    Gender = x.Gender,
                    Name = x.Name,
                    Password = x.Password,
                    PhoneNumber = x.PhoneNumber,
                    UserName = x.UserName,
                }).ToListAsync();
            if (customer.Any())
            {
                var customers = customer.First();
                return new BaseResponse<IList<CustomerDto>>
                {
                    Success = true,
                    Message = $"Customer with ID {Id} retrieved successfully.",
                    Data = customer
                };
            }
            else
            {
                return new BaseResponse<IList<CustomerDto>>
                {
                    Success = false,
                    Message = $"Failed to retrieve customer with ID {Id}.The customer may not exist or there was an error in the retrieval process.",

                    Hasherror = true
                };
            }
        }


        public async Task<List<CustomerDto>> GetCustomer()
        {
            return _dbContext.Customers
                //.Include(x => x.Id)
                //.Include(x => x.RoomType)
                .Select(x => new CustomerDto()
                {
                    Id = x.Id,
                    Address = x.Address,
                    Age = x.Age,
                    Email = x.Email,
                    Gender = x.Gender,
                    Name = x.Name,
                    Password = x.Password,
                    PhoneNumber = x.PhoneNumber,
                    UserName = x.UserName,
                }).ToList();
        }


        public async Task<BaseResponse<CustomerDto>> GetCustomerAsync(Guid Id)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == Id);
            if (customer != null)
            {
                return new BaseResponse<CustomerDto>
                {
                    Message = "",
                    Success = true,
                    Data = new CustomerDto
                    {
                        Address = customer.Address,
                        Age = customer.Age,
                        Email = customer.Email,
                        Gender = customer.Gender,
                        Name = customer.Name,
                        Password = customer.Password,
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
            var customer = await _dbContext.Customers
               .Select(x => new CustomerDto
               {
                   Address = x.Address,
                   Age = x.Age,
                   Email = x.Email,
                   Gender = x.Gender,
                   Name = x.Name,
                   Password = x.Password,
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

        public async Task<BaseResponse<CustomerDto>> UpdateCustomer(Guid Id, UpdateCustomer request)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync();
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
            request.Age = request.Age;
            request.Email = request.Email;
            request.Gender = request.Gender;
            request.Password = request.Password;
            _dbContext.Customers.Update(customer);
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


    }

}
