﻿using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
