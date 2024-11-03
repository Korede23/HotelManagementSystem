using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using HotelManagementSystem.Model.Entity.Enum;
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
        private readonly ILogger<UserService> _logger;

        public UserService(ApplicationDbContext dbContext, UserManager<User> userManager,
                            SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager,
                            ICustomerServices customerService, ILogger<UserService> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _customerServices = customerService;
            _logger = logger;
        }

        public async Task<BaseResponse<Guid>> CreateUser(CreateUser request)
        {
            _logger.LogInformation("Creating user with username: {UserName}", request.UserName);
            try
            {
                if (request != null)
                {
                    // Set role to "Customer" if the request role is null or if the user is not an admin
                    var roleToAssign = request.Role ?? UserRole.Customer;

                    var user = new User()
                    {
                        FullName = $"{request.LastName} {request.FirstName}",
                        UserName = request.UserName,
                        Address = request.Address,
                        DateOfBirth = request.DateOfBirth,
                        Email = request.Email,
                        Gender = request.Gender,
                        PhoneNumber = request.PhoneNumber,
                        UserRole = roleToAssign,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        AgeRange = request.AgeRange
                    };

                    var result = await _userManager.CreateAsync(user, request.Password);

                    if (result.Succeeded)
                    {
                        var addUserRole = await _userManager.AddToRoleAsync(user, roleToAssign.ToString());

                        if (addUserRole.Succeeded)
                        {
                            return new BaseResponse<Guid>
                            {
                                Success = true,
                                Message = $"{roleToAssign} created successfully",
                                Data = Guid.Parse(user.Id)
                            };
                        }
                    }
                }

                _logger.LogWarning("User creation failed for username: {UserName}", request.UserName);
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "User creation failed"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the user with username: {UserName}", request.UserName);
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "User creation failed"
                };
            }
        }


        public async Task<BaseResponse<Guid>> DeleteUserAsync(string id)
        {
            _logger.LogInformation("Deleting user with ID: {UserId}", id);
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (user != null)
                {
                    _dbContext.Users.Remove(user);
                }

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    _logger.LogInformation("User deleted successfully with ID: {UserId}", id);
                    return new BaseResponse<Guid>
                    {
                        Success = true,
                        Message = "User deleted successfully"
                    };
                }
                else
                {
                    _logger.LogWarning("Failed to delete user with ID: {UserId}", id);
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "Delete Failed, unable to process the deletion of User at this time"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the user with ID: {UserId}", id);
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "Delete Failed, unable to process the deletion of User at this time"
                };
            }
        }

        public async Task<BaseResponse<UserDto>> GetUserByIdAsync(string id)
        {
            _logger.LogInformation("Retrieving user with ID: {UserId}", id);
            try
            {
                var user = await _dbContext.Users
                    .Where(x => x.Id == id)
                    .Select(x => new UserDto()
                    {
                        Address = x.Address,
                        AgeRange = x.AgeRange,
                        Email = x.Email,
                        Gender = x.Gender,
                        Name = x.FullName,
                        PhoneNumber = x.PhoneNumber,
                        Id = x.Id,
                        UserName = x.UserName,
                    }).FirstOrDefaultAsync();

                if (user != null)
                {
                    _logger.LogInformation("User retrieved successfully with ID: {UserId}", id);
                    return new BaseResponse<UserDto>
                    {
                        Success = true,
                        Message = "User retrieved successfully",
                        Data = user
                    };
                }

                _logger.LogWarning("User not found with ID: {UserId}", id);
                return new BaseResponse<UserDto>
                {
                    Success = false,
                    Message = "User not found"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the user with ID: {UserId}", id);
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
            _logger.LogInformation("Retrieving user with ID: {UserId}", Id);
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (user != null)
            {
                _logger.LogInformation("User retrieved successfully with ID: {UserId}", Id);
                return new BaseResponse<UserDto>
                {
                    Message = "User retrieved successfully",
                    Success = true,
                    Data = new UserDto
                    {
                        Id = user.Id,
                        Address = user.Address,
                        AgeRange = user.AgeRange,
                        Email = user.Email,
                        Gender = user.Gender,
                        Name = user.FullName,
                        PhoneNumber = user.PhoneNumber,
                        UserName = user.UserName,
                    }
                };
            }
            _logger.LogWarning("User not found with ID: {UserId}", Id);
            return new BaseResponse<UserDto>
            {
                Success = false,
                Message = "User not found",
            };
        }

        public async Task<BaseResponse<IList<UserDto>>> GetAllUserAsync()
        {
            _logger.LogInformation("Retrieving all users");
            try
            {
                var users = await _dbContext.Users
                    .Select(x => new UserDto
                    {
                        Address = x.Address,
                        AgeRange = x.AgeRange,
                        Email = x.Email,
                        Gender = x.Gender,
                        Name = x.FullName,
                        PhoneNumber = x.PhoneNumber,
                        Id = x.Id,
                        UserName = x.UserName,
                    }).ToListAsync();

                _logger.LogInformation("Users retrieved successfully");
                return new BaseResponse<IList<UserDto>>
                {
                    Success = true,
                    Message = "Users retrieved successfully",
                    Data = users
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all users");
                return new BaseResponse<IList<UserDto>>
                {
                    Success = false,
                    Message = "Failed to retrieve users"
                };
            }
        }

        public async Task<List<UserDto>> GetUser()
        {
            _logger.LogInformation("Retrieving all users as list");
            return await _dbContext.Users
                .Select(x => new UserDto
                {
                    Name = x.FullName,
                    Address = x.Address,
                    Gender = x.Gender,
                    AgeRange = x.AgeRange,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Id = x.Id,
                    UserName = x.UserName,
                }).ToListAsync();
        }

        public async Task<BaseResponse<IList<UserDto>>> UpdateUser(string Id, UpdateUser request)
        {
            _logger.LogInformation("Updating user with ID: {UserId}", Id);
            try
            {
                var user = _dbContext.Users.FirstOrDefault(x => x.Id == Id);
                if (user == null)
                {
                    _logger.LogWarning("User not found with ID: {UserId}", Id);
                    return new BaseResponse<IList<UserDto>>
                    {
                        Success = false,
                        Message = "Update Failed"
                    };
                }

                user.FullName = request.Name;
                user.PhoneNumber = request.PhoneNumber;
                user.UserName = request.UserName;
                user.Email = request.Email;
                user.Address = request.Address;
                user.AgeRange = request.AgeRange;

                _dbContext.Users.Update(user);

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    _logger.LogInformation("User updated successfully with ID: {UserId}", Id);
                    return new BaseResponse<IList<UserDto>>
                    {
                        Success = true,
                        Message = "User Updated successfully"
                    };
                }
                else
                {
                    _logger.LogWarning("Failed to update user with ID: {UserId}", Id);
                    return new BaseResponse<IList<UserDto>>
                    {
                        Success = false,
                        Message = "Update Failed"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user with ID: {UserId}", Id);
                return new BaseResponse<IList<UserDto>>
                {
                    Success = false,
                    Message = "Update Failed"
                };
            }
        }

        public async Task LogOutAsync()
        {
            _logger.LogInformation("Logging out user");
            await _signInManager.SignOutAsync();
        }

        public async Task<Status> ChangePasswordAsync(ChangePasswordModel model, string UserName)
        {
            _logger.LogInformation("Changing password for user with username: {UserName}", UserName);
            var status = new Status();

            var user = await _userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                _logger.LogWarning("User not found with username: {UserName}", UserName);
                status.Message = "User does not exist";
                status.StatusCode = 0;
                return status;
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                _logger.LogInformation("Password updated successfully for user with username: {UserName}", UserName);
                status.Message = "Password has been updated successfully";
                status.StatusCode = 1;
            }
            else
            {
                _logger.LogWarning("Failed to update password for user with username: {UserName}", UserName);
                status.Message = "Some error occurred";
                status.StatusCode = 0;
            }
            return status;
        }

        public async Task<Status> LoginAsync(LoginModel model)
        {
            _logger.LogInformation("Logging in user with username: {UserName}", model.UserName);
            var status = new Status();
            try
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    if (!await _userManager.CheckPasswordAsync(user, model.Password))
                    {
                        _logger.LogWarning("Invalid password for user with username: {UserName}", model.UserName);
                        status.StatusCode = 0;
                        status.Message = "Invalid Password or Username";
                        return status;
                    }

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

                        _logger.LogInformation("User logged in successfully with username: {UserName}", model.UserName);

                        if (Enum.TryParse(typeof(UserRole), userRoles.FirstOrDefault(), out var parsedRole))
                        {
                            status.Role = (UserRole?)parsedRole;
                        }
                        else
                        {
                            status.Role = null;
                        }

                        status.StatusCode = 1;
                        status.Success = true;
                        status.Message = "Logged in successfully";
                    }
                    else if (signInResult.IsLockedOut)
                    {
                        _logger.LogWarning("User is locked out with username: {UserName}", model.UserName);
                        status.StatusCode = 0;
                        status.Message = "User is locked out";
                    }
                    else
                    {
                        _logger.LogWarning("Error occurred during login for user with username: {UserName}", model.UserName);
                        status.StatusCode = 0;
                        status.Message = "Error on logging in";
                    }

                    return status;
                }
                _logger.LogWarning("Invalid login details for user with username: {UserName}", model.UserName);
                status.StatusCode = 0;
                status.Message = "Invalid login details";
                return status;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in the user with username: {UserName}", model.UserName);
                status.StatusCode = 0;
                status.Message = "Error occurred while processing your request";
                return status;
            }
        }


    }
}
