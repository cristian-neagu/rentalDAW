using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Rental.Data;
using Rental.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Rental.Controllers
{
    [Route("api/")]
    public class MakeReservationAPIController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public MakeReservationAPIController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: api/getCars
        [HttpGet]
        [Route("getCars")]
        public async Task<ResponseModel> GetCars()
        {
            var cars = await _context.Cars.Include(c => c.CarType).Include(r => r.Reservations).AsNoTracking().ToListAsync();
            foreach(var car in cars)
            {
                car.CarType.Cars = null;
                foreach(var res in car.Reservations)
                {
                    res.Car = null;
                }
            }
            ResponseModel resp = new ResponseModel();
            resp.Message = "Success";
            resp.Status = true;
            resp.Data = cars;

            return resp;
        }

        // POST api/
        [HttpPost]
        [Route("setReservation")]
        public async Task<bool> Post(DateTime start, DateTime end, string observations, int car)
        {
            int count = 0;
            var reservations = _context.Reservation.Where(r => (r.Car.CarId == car)).ToList();
            foreach(var res in reservations)
            {
                if(res.StartDate >= start && res.EndDate <= end)
                {
                    count++;
                } else if(res.StartDate < start && res.EndDate > end)
                {
                    count++;
                } else if(res.StartDate < start && res.EndDate < end && res.EndDate > start)
                {
                    count++;
                } else if(res.StartDate > start && res.EndDate > end && res.StartDate < end)
                {
                    count++;
                }
            }

            if(count != 0)
            {
                return false;
            } else
            {
                var carSelected = _context.Cars.Where(c => c.CarId == car).First();
                Reservation newRes = new Reservation();
                newRes.StartDate = start;
                newRes.EndDate = end;
                newRes.Car = carSelected;
                newRes.Observations = observations;
                newRes.User = await GetCurrentUserAsync();
                _context.Add(newRes);
                await _context.SaveChangesAsync();

                return true;
            }
        }
    }
}
