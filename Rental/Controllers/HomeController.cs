using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rental.Models;

namespace Rental.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["showUserLinks"] = User.IsInRole("user");
            ViewData["showAdminLinks"] = User.IsInRole("admin");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            ViewData["showUserLinks"] = User.IsInRole("user");
            ViewData["showAdminLinks"] = User.IsInRole("admin");
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            ViewData["showUserLinks"] = User.IsInRole("user");
            ViewData["showAdminLinks"] = User.IsInRole("admin");
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
