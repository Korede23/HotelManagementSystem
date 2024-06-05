using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Dto.Implementation.Services
{
    public class UserService : IUserServices
    {
        private readonly ApplicationDbContext _dbContext;
        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BaseResponse<Guid>> CreateUser(CreateUser request)
        {

            if (request != null)
            {
                var user = new User()
                {
                    Name = request.Name,
                    UserName = request.UserName,
                    Address = request.Address,
                    Age = request.Age,
                    CreatedTime = DateTime.Now,
                    Email = request.Email,
                    Gender = request.Gender,
                    Password = request.Password,
                    PhoneNumber = request.PhoneNumber,

                };
                _dbContext.Users.Add(user);


                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return new BaseResponse<Guid>

                    {
                        Success = true,
                        Message = "User Created Successfully"
                    };
                }
            }
            return new BaseResponse<Guid>

            {
                Success = false,
                Message = "User Created failed"
            };

        }

        public async Task<BaseResponse<Guid>> DeleteUserAsync(Guid id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
            }

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return new BaseResponse<Guid>
                {
                    Success = true,
                    Message = "User deleted succesfully",


                };
            }

            else
            {
                return new BaseResponse<Guid>

                {
                    Success = false,
                    Message = "Delete Failed, unable to process the deletion of User at this time"
                };
            }
        }





        //public bool DeleteUser(int id)
        //{
        //    var user = _dbContext.Users.FirstOrDefault(x => x.Id == id);
        //    if (user != null)
        //    {
        //        _dbContext.Users.Remove(user);
        //    }
        //    return _dbContext.SaveChanges() > 0 ? true : false;
        //}

        public async Task<BaseResponse<IList<UserDto>>> GetUserByIdAsync(Guid Id)
        {
            try
            {

                var user = await _dbContext.Users
                .Where(x => x.Id == Id)
                .Select(x => new UserDto()
                {
                    Address = x.Address,
                    Age = x.Age,
                    Email = x.Email,
                    Gender = x.Gender,
                    Name = x.Name,
                    Password = x.Password,
                    PhoneNumber = x.PhoneNumber,
                    UserId = x.Id,
                    UserName = x.UserName,
                }).ToListAsync();
                if (user != null)
                {
                    return new BaseResponse<IList<UserDto>>
                    {
                        Success = true,
                        Message = "User retrieved successfully",
                        Data = user

                    };
                }

                return new BaseResponse<IList<UserDto>>
                {
                    Success = false,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IList<UserDto>>
                {
                    Success = false,
                    Message = "Failed to retrieve user ",
                    Hasherror = true
                };

            }

        }

        public async Task<BaseResponse<UserDto>> GetUserAsync(Guid Id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (user != null)
            {
                return new BaseResponse<UserDto>
                {
                    Message = "",
                    Success = true,
                    Data = new UserDto
                    {
                        
                        UserId = user.Id,
                        Address = user.Address,
                        Age = user.Age,
                        Email = user.Email,
                        Gender = user.Gender,
                        Name = user.Name,
                        PhoneNumber = user.PhoneNumber,
                        UserName = user.UserName,
                    }

                };
            }
            return new BaseResponse<UserDto>
            {
                Success = false,
                Message = "",
            };
        }


        public async Task<BaseResponse<IList<UserDto>>> GetAllUserAsync()
        {
            try
            {
                var user = await _dbContext.Users
               .Select(x => new UserDto
               {
                   Address = x.Address,
                   Age = x.Age,
                   Email = x.Email,
                   Gender = x.Gender,
                   Name = x.Name,
                   Password = x.Password,
                   PhoneNumber = x.PhoneNumber,
                   UserId = x.Id,
                   UserName = x.UserName,
               }).ToListAsync();
                return new BaseResponse<IList<UserDto>>
                {
                    Success = true,
                    Message = "Users retrieved successfully",
                    Data = user
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IList<UserDto>>
                {
                    Success = false,
                    Message = "Failed to retrieve users"
                };
            }

        }



        public async Task<List<UserDto>> GetUser()
        {
            return _dbContext.Users
                .Select(x => new UserDto()
                {
                    
                    Name = x.Name,
                    Address = x.Address,
                    Gender = x.Gender,
                    Age = x.Age,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    UserId = x.Id,
                    UserName = x.UserName,
                    Password = x.Password

                }).ToList();
        }



        public async Task<BaseResponse<IList<UserDto>>> UpdateUser(Guid Id, UpdateUser request)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == Id);
            if (user == null)
            {
                return new BaseResponse<IList<UserDto>>
                {
                    Success = false,
                    Message = "Update Failed"
                };
            }

            user.Name = request.Name;
            user.Password = request.Password;
            user.PhoneNumber = request.PhoneNumber;
            user.UserName = request.UserName;
            user.Password = request.Password;
            user.PhoneNumber = request.PhoneNumber;
            user.Email = request.Email;
            user.Address = request.Address;
            user.Age = request.Age;
            _dbContext.Users.Update(user);

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return new BaseResponse<IList<UserDto>>
                {
                    Success = true,
                    Message = "User Updated succesfully"
                };
            }
            return new BaseResponse<IList<UserDto>>
            {
                Success = false,
                Message = "Update Failed"
            };
        }
    }
}



