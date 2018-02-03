using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rental.Data;
using Rental.Models;

namespace Rental.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cars = await _context.Cars
                .Include(c => c.CarType)
                .Include(c => c.Reservations)
                .AsNoTracking()
                .ToListAsync();
            IDictionary<int, int> carTypes = new Dictionary<int, int>();
            foreach (var car in cars) {
                if (carTypes.ContainsKey(car.CarType.Id))
                {
                    carTypes[car.CarType.Id] += car.Reservations.Count;
                } else
                {
                    carTypes[car.CarType.Id] = car.Reservations.Count;
                }
            }

            var carTypeList = carTypes.ToList();
            carTypeList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

            List<CarType> topModels = new List<CarType>();
            int i = 0;
            foreach(var carModel in carTypeList)
            {
                if(i < 3)
                {
                    var carType = await _context.CarType
                        .Where(c => c.Id == carModel.Key)
                        .AsNoTracking()
                        .FirstAsync();
                    topModels.Add(carType);
                    i++;
                }
            }

            ViewData["showUserLinks"] = User.IsInRole("user");
            ViewData["showAdminLinks"] = User.IsInRole("admin");
            return View(topModels);
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
