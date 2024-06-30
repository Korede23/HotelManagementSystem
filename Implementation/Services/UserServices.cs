﻿using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HotelManagementSystem.Dto.Implementation.Services
{
    public class UserService : IUserServices
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICustomerServices _customerServices;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(ApplicationDbContext dbContext, UserManager<User> userManager,
                            SignInManager<User> signInManager,
                           RoleManager<IdentityRole> roleManager,
                            ICustomerServices customerService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _customerServices = customerService;
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
                    DateOfBirth = request.DateOfBirth,
                    Email = request.Email,
                    Gender = request.Gender,
                    PhoneNumber = request.PhoneNumber,

                };

                var result = await _userManager.CreateAsync(user, request.Password);


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

        public async Task<BaseResponse<Guid>> DeleteUserAsync(string id)
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

        //public async Task<BaseResponse<UserDto>> GetUserByIdAsync(Guid Id)
        //{
        //    try
        //    {

        //        var user = await _dbContext.Users
        //        .Where(x => x.Id == Id)
        //        .Select(x => new UserDto()
        //        {
        //            Address = x.Address,
        //            DateOfBirth = x.DateOfBirth,
        //            Email = x.Email,
        //            Gender = x.Gender,
        //            Name = x.Name,
        //            Password = x.Password,
        //            PhoneNumber = x.PhoneNumber,
        //            Id = x.Id,
        //            UserName = x.UserName,
        //        }).ToListAsync();
        //        if (user != null)
        //        {
        //            return new BaseResponse<UserDto>
        //            {
        //                Success = true,
        //                Message = "User retrieved successfully",
        //                Data = user

        //            };
        //        }

        //        return new BaseResponse<UserDto>
        //        {
        //            Success = false,
        //            Message = ""
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new BaseResponse<UserDto>
        //        {
        //            Success = false,
        //            Message = "Failed to retrieve user ",
        //            Hasherror = true
        //        };

        //    }

        //}

        public async Task<BaseResponse<UserDto>> GetUserByIdAsync(string id)
        {
            try
            {
                var user = await _dbContext.Users
                    .Where(x => x.Id == id)
                    .Select(x => new UserDto()
                    {
                        Address = x.Address,
                        DateOfBirth = x.DateOfBirth,
                        Email = x.Email,
                        Gender = x.Gender,
                        Name = x.Name,
                        PhoneNumber = x.PhoneNumber,
                        Id = x.Id,
                        UserName = x.UserName,
                    }).FirstOrDefaultAsync();

                if (user != null)
                {
                    return new BaseResponse<UserDto>
                    {
                        Success = true,
                        Message = "User retrieved successfully",
                        Data = user
                    };
                }

                return new BaseResponse<UserDto>
                {
                    Success = false,
                    Message = "User not found"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserDto>
                {
                    Success = false,
                    Message = "Failed to retrieve user",
                    Hasherror = true
                };
            }
        }

        public async Task<BaseResponse<UserDto>> GetUserAsync(string Id)
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

                        Id = user.Id,
                        Address = user.Address,
                        DateOfBirth = user.DateOfBirth,
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
                   DateOfBirth = x.DateOfBirth,
                   Email = x.Email,
                   Gender = x.Gender,
                   Name = x.Name,
                   PhoneNumber = x.PhoneNumber,
                   Id = x.Id,
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
                    DateOfBirth = x.DateOfBirth,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Id = x.Id,
                    UserName = x.UserName,

                }).ToList();
        }



        public async Task<BaseResponse<IList<UserDto>>> UpdateUser(string Id, UpdateUser request)
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
            user.PhoneNumber = request.PhoneNumber;
            user.UserName = request.UserName;
            user.PhoneNumber = request.PhoneNumber;
            user.Email = request.Email;
            user.Address = request.Address;
            user.DateOfBirth = request.DateOfBirth;
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

        public Task<BaseResponse<Guid>> DeleteUserAsync(Guid id)
        {
            throw new NotImplementedException();
        }

       

        public Task<BaseResponse<UserDto>> GetUserAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<IList<UserDto>>> UpdateUser(Guid Id, UpdateUser request)
        {
            throw new NotImplementedException();
        }

        //public async Task<Status> LoginAsync(LoginModel model)
        //{
        //    var status = new Status();
        //    try
        //    {
        //        var user = await _userManager.FindByNameAsync(model.UserName);
        //        if (user == null)
        //        {
        //            status.StatusCode = 0;
        //            status.Message = "Invalid Password or Username";
        //            return status;
        //        } 

        //        if (!await _userManager.CheckPasswordAsync(user, model.Password))
        //        {
        //            status.StatusCode = 0;
        //            status.Message = "Invalid Password or Username";
        //            return status;
        //        }

        //        var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, true, true);
        //        if (signInResult.Succeeded)
        //        {
        //            var userRoles = await _userManager.GetRolesAsync(user);
        //            var authClaims = new List<Claim>
        //        {
        //            new Claim(ClaimTypes.Name, user.UserName),
        //        };

        //            foreach (var userRole in userRoles)
        //            {
        //                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        //            }
        //            status.StatusCode = 1;
        //            status.Message = "Logged in successfully";
        //        }
        //        else if (signInResult.IsLockedOut)
        //        {
        //            status.StatusCode = 0;
        //            status.Message = "User is locked out";
        //        }
        //        else
        //        {
        //            status.StatusCode = 0;
        //            status.Message = "Error on logging in";
        //        }

        //        return status;
        //    }
        //    catch (Exception)
        //    {
        //        status.StatusCode = 0;
        //        status.Message = "error occured while processing your request";
        //        return status;
        //    }
        //}

        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
         
        public async Task<Status> ChangePasswordAsync(ChangePasswordModel model, string username)
        {
            var status = new Status();

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                status.Message = "User does not exist";
                status.StatusCode = 0;
                return status;
            }
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                status.Message = "Password has updated successfully";
                status.StatusCode = 1;
            }
            else
            {
                status.Message = "Some error occured";
                status.StatusCode = 0;
            }
            return status;

        }
        public async Task<Status> LoginAsync(LoginModel model)
        {
            var status = new Status();
            try
            {
                // Attempt to find the user
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    // Check the user's password
                    if (!await _userManager.CheckPasswordAsync(user, model.Password))
                    {
                        status.StatusCode = 0;
                        status.Message = "Invalid Password or Username";
                        return status;
                    }

                    // Attempt to sign in the user
                    var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, true, true);
                    if (signInResult.Succeeded)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                };

                        foreach (var userRole in userRoles)
                        {
                            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                        }

                        status.StatusCode = 1;
                        status.Message = "Logged in successfully";
                    }
                    else if (signInResult.IsLockedOut)
                    {
                        status.StatusCode = 0;
                        status.Message = "User is locked out";
                    }
                    else
                    {
                        status.StatusCode = 0;
                        status.Message = "Error on logging in";
                    }

                    return status;
                }
                else
                {
                    // If user is not found, attempt to log in as a customer
                    var customerStatus = await _customerServices.CustomerLogin(model);
                    if (customerStatus.StatusCode == 1)
                    {
                        return customerStatus;
                    }
                    else
                    {
                        status.StatusCode = 0;
                        status.Message = "Invalid Password or Username";
                        return status;
                    }
                }
            }
            catch (Exception)
            {
                status.StatusCode = 0;
                status.Message = "Error occurred while processing your request";
                return status;
            }
        }

    }

}




