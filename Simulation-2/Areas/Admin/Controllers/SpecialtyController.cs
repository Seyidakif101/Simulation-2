using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simulation_2.Context;
using Simulation_2.Helper;
using Simulation_2.Models;
using Simulation_2.ViewModels.SpecialtyViewModels;
using Simulation_2.ViewModels.TrainerViewModels;

namespace Simulation_2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SpecialtyController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var specialties = await _context.Specialties.Select(x => new SpecialtyGetVM()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            return View(specialties);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Create(SpecialtyCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
           
            Specialty specialty = new()
            {
                Name = vm.Name
            };
            await _context.Specialties.AddAsync(specialty);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var specialty = await _context.Specialties.FindAsync(id);

            if (specialty is null)
                return NotFound();

            SpecialtyUpdateVM vm = new()
            {
                Id = specialty.Id,
                Name = specialty.Name
            };


            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> Update(SpecialtyUpdateVM vm)
        {

            if (!ModelState.IsValid)
                return View(vm);

           
            var existSpecialty = await _context.Specialties.FindAsync(vm.Id);

            if (existSpecialty is null)
                return BadRequest();

            existSpecialty.Name = vm.Name;


           


            _context.Specialties.Update(existSpecialty);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var specialty = await _context.Specialties.FindAsync(id);

            if (specialty is null)
                return NotFound();

            _context.Specialties.Remove(specialty);
            await _context.SaveChangesAsync();


            return RedirectToAction("Index");

        }


    }
}
