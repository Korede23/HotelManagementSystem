using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Implementation.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;
       

        public UserController(IUserServices  userService)
        {
            _userServices = userService;
        }


        [HttpGet("get-user")]
        public async Task<IActionResult> Index()
        {
            var user = await _userServices.GetUser();
            return View(user);
        }


        public async Task<IActionResult> Create()
        {
            var user = await _userServices.GetAllUserAsync();
            if (user.Success)
            {

                return View();
            }
            return BadRequest();
        }




        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser(CreateUser request)
        {
            
         var user = await   _userServices.CreateUser(request);
            if (user.Success)
            {
                return RedirectToAction("Index");
            }
            return BadRequest();
        }


        [HttpGet("edit-user/{id}")]
        public async Task<IActionResult> EditUser([FromRoute] Guid id)
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
                return RedirectToAction("Index");
            }
            return View(user);
        }


        [HttpGet("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var user = await _userServices.DeleteUserAsync(id);
            if (user.Success)
            {
                return RedirectToAction("Index", "User");
            }
            return BadRequest(user);
        }



        [HttpGet("get-all-user-created")]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _userServices.GetAllUserAsync();
            if (users.Success)
            {
                return RedirectToAction("Index");
            }
            return View(users);

        }



        [HttpGet("get-user-by-id/{id}")]
        public async Task<IActionResult> GetUserByIdAsync(Guid id)
        {
            var user = await _userServices.GetUserByIdAsync(id);
            if (user.Success)
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }



    }
}
