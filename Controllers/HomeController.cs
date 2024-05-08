using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
//using AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MyCourse.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            //return Content("Sono nella Index dell'Homepage");
            ViewData["Title"] = "Home";
            return View();
        }

        public IActionResult Error()
        {
            return View("Error!!!");
        } 
    }
}