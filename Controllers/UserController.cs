using AspNetCoreHero.ToastNotification.Abstractions;
using Azure;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Implementation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HotelManagementSystem.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly INotyfService _notyf;
        private readonly IEmailService _emailService;

        public UserController(IUserServices  userService , INotyfService notyf,IEmailService emailService)
        {
            _userServices = userService;
            _notyf = notyf;
            _emailService = emailService;
        }

        [Authorize]
        [HttpGet("get-user")]
        public async Task<IActionResult> Users()
        {
            var user = await _userServices.GetUser();
            return View(user);
        }

        [AllowAnonymous]
        [HttpGet("create-user")]
        public async Task<IActionResult> CreateUser()
        {
            var user = await _userServices.GetAllUserAsync();
            if (user.Success)
            {

                return View();
            }
            return BadRequest();
        }


        [AllowAnonymous]
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser(CreateUser request , CreateUser profile)
        {
            var userResponse = await _userServices.CreateUser(request);

            if (userResponse.Success)
            {
                var emailResponse = await _emailService.SendMessageToUserAsync(profile);

                if (emailResponse.Success)
                {
                    _notyf.Success(userResponse.Message, 3);
                    _notyf.Success(emailResponse.Message, 3);
                    return RedirectToAction("Login");
                }
                else
                {
                    _notyf.Success(userResponse.Message, 3);
                    _notyf.Warning("User created but failed to send welcome email.", 3);
                    return RedirectToAction("Login"); 
                }
            }
            else
            {
                _notyf.Error(userResponse.Message, 3);
                return BadRequest(userResponse);
            }
        }



        [HttpGet("edit-user/{id}")]
        public async Task<IActionResult> EditUser([FromRoute] string id)
        {
            var user = await _userServices.GetUserAsync(id);

            return View(user.Data);
        }

         
        [HttpPost("edit-user/{id}")]
        public async Task<IActionResult> EditUser(UpdateUser request)
        {
           var user = await _userServices.UpdateUser(request.Id, request);
            if (user.Success)
            {
                _notyf.Success(user.Message, 3);
                return RedirectToAction("Users");
            }
            _notyf.Error(user.Message, 3);
            return View(user);
        }


        [HttpGet("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            var user = await _userServices.DeleteUserAsync(id);
            if (user.Success)
            {
                _notyf.Success(user.Message, 3);
                return RedirectToAction("Users", "User");
            }
            _notyf.Error(user.Message, 3);
            return BadRequest(user);
        }



        [HttpGet("get-all-user-created")]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _userServices.GetAllUserAsync();
            if (users.Success)
            {
                _notyf.Success(users.Message, 3);
                return RedirectToAction("Users");
            }
            return View(users);

        }



        [HttpGet("get-user-by-id/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userServices.GetUserByIdAsync(id);
            if (user.Success && user.Data != null)
            {
                _notyf.Success(user.Message, 3);
                return View(user.Data); 
            }
            return RedirectToAction("Users"); 
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _userServices.LoginAsync(model);
            if (result.StatusCode == 1)
            {
                _notyf.Success(result.Message, 3);
                return RedirectToAction("Index", "Admin" );
            } 
            else
            {
                TempData["msg"] = result.Message;
                _notyf.Error(result.Message, 4);
                return RedirectToAction(nameof(Login));
            }
        }

        [AllowAnonymous]
        [HttpGet("change-password")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordModel changePasswordModelDto, string UserName)
        {
            var result = await _userServices.ChangePasswordAsync(changePasswordModelDto, UserName);
            return RedirectToAction(nameof(Login));
        }


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _userServices.LogOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
