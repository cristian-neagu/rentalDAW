using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rental.Data;
using Rental.Models;

namespace Rental.Controllers
{
    public class CarTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CarTypes
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            ViewData["showUserLinks"] = User.IsInRole("user");
            ViewData["showAdminLinks"] = User.IsInRole("admin");
            return View(await _context.CarType.ToListAsync());
        }

        // GET: CarTypes/Details/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carType = await _context.CarType
                .Include(c => c.Cars)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (carType == null)
            {
                return NotFound();
            }

            ViewData["showUserLinks"] = User.IsInRole("user");
            ViewData["showAdminLinks"] = User.IsInRole("admin");
            return View(carType);
        }

        // GET: CarTypes/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewData["showUserLinks"] = User.IsInRole("user");
            ViewData["showAdminLinks"] = User.IsInRole("admin");
            return View();
        }

        // POST: CarTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Id,Make,Model,FullType,Description,Image")] CarType carType, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file == null || file.Length == 0)
                    return Content("file not selected");

                var path = Path.Combine(
                            Directory.GetCurrentDirectory(), "wwwroot", "images", "carTypes",
                            file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                carType.Image = "/images/carTypes/" + file.FileName;

                _context.Add(carType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carType);
        }

        // GET: CarTypes/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carType = await _context.CarType.SingleOrDefaultAsync(m => m.Id == id);
            if (carType == null)
            {
                return NotFound();
            }
            ViewData["showUserLinks"] = User.IsInRole("user");
            ViewData["showAdminLinks"] = User.IsInRole("admin");
            return View(carType);
        }

        // POST: CarTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Make,Model,FullType,Description,Image")] CarType carType, IFormFile file)
        {
            if (id != carType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (file != null || file.Length != 0)
                {
                    var path = Path.Combine(
                                Directory.GetCurrentDirectory(), "wwwroot", "images", "carTypes",
                                file.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    carType.Image = "/images/carTypes/" + file.FileName;
                }
                try
                {
                    _context.Update(carType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarTypeExists(carType.Id))
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
            return View(carType);
        }

        // GET: CarTypes/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carType = await _context.CarType
                .SingleOrDefaultAsync(m => m.Id == id);
            if (carType == null)
            {
                return NotFound();
            }

            ViewData["showUserLinks"] = User.IsInRole("user");
            ViewData["showAdminLinks"] = User.IsInRole("admin");
            return View(carType);
        }

        // POST: CarTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carType = await _context.CarType.SingleOrDefaultAsync(m => m.Id == id);
            _context.CarType.Remove(carType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarTypeExists(int id)
        {
            return _context.CarType.Any(e => e.Id == id);
        }
    }
}
