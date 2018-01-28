using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rental.Data;
using Rental.Models;

namespace Rental.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cars
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["MakeSortParam"] = String.IsNullOrEmpty(sortOrder) ? "make" : "";
            ViewData["ModelSortParam"] = String.IsNullOrEmpty(sortOrder) ? "model" : "";
            ViewData["ColorSortParam"] = String.IsNullOrEmpty(sortOrder) ? "color" : "";
            ViewData["CurrentFilter"] = searchString;
            var cars = _context.Cars.Include(b => b.CarType).AsQueryable();
            if (!String.IsNullOrEmpty(searchString))
            {
                cars = cars.Where(s => s.CarType.Make.Contains(searchString)
                                       || s.CarType.Model.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "make":
                    cars = cars.OrderBy(c => c.CarType.Make);
                    break;
                case "model":
                    cars = cars.OrderBy(c => c.CarType.Model);
                    break;
                case "color":
                    cars = cars.OrderBy(c => c.Color);
                    break;
                default:
                    cars = cars.OrderBy(c => c.LicenseNumber);
                    break;
            }

            ViewData["showUserLinks"] = User.IsInRole("user");
            ViewData["showAdminLinks"] = User.IsInRole("admin");
            return View(cars.AsNoTracking().AsEnumerable());
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Reservations)
                .Include(c => c.CarType)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            PopulateCarTypesDropDownList();
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarId,LicenseNumber,Color,Seats,Doors,AirCon,Available,CarType,PricePerDay")] Car car)
        {
            if (ModelState.IsValid)
            {
                var selected =_context.CarType.Single(ct => ct.Id == car.CarType.Id);
                car.CarType = selected;
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateCarTypesDropDownList(car.CarType.Id);
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.Include(m => m.CarType).SingleOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }
            PopulateCarTypesDropDownList(car.CarType.Id);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarId,LicenseNumber,Color,Seats")] Car car)
        {
            if (id != car.CarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.CarId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateCarTypesDropDownList(car.CarType.Id);
            return View(car);
        }

        private void PopulateCarTypesDropDownList(object selectedCarType = null)
        {
            var carTypeQuery = from c in _context.CarType
                                   orderby c.Make
                                   select c;
            ViewBag.CarType = new SelectList(carTypeQuery.AsNoTracking(), "Id", "FullType", selectedCarType);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(m => m.CarType)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.AsNoTracking().SingleOrDefaultAsync(m => m.CarId == id);
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }
    }
}
