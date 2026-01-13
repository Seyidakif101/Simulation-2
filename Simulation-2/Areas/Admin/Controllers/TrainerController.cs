using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Simulation_2.Context;
using Simulation_2.Helper;
using Simulation_2.Models;
using Simulation_2.ViewModels.TrainerViewModels;

namespace Simulation_2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TrainerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly string _folderPath;
        public TrainerController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _folderPath = Path.Combine(_environment.WebRootPath, "images");
        }
        public async Task<IActionResult> Index()
        {
            var trainers = await _context.Trainers.Select(x => new TrainerGetVM()
            {
                Id = x.Id,
                Name = x.Name,
                ImagePath = x.ImagePath,
                SpecialtyName=x.Specialty.Name
            }).ToListAsync();

            return View(trainers);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await _sendSpecialtiesWithViewBag();
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Create(TrainerCreateVM vm)
        {
            await _sendSpecialtiesWithViewBag();
           if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var isExistSpecialty = await _context.Specialties.AnyAsync(x => x.Id == vm.SpecialtyId);
            if (!isExistSpecialty)
            {
                ModelState.AddModelError("SpecialtyId", "Bele Specialty yoxdur.");
                return View(vm);
            }
            if (!vm.Image.CheckSize(2))
            {
                ModelState.AddModelError("Image", "Olcu 2mb boyukdue");
                return View(vm);
            }
            if (!vm.Image.CheckType("image"))
            {
                ModelState.AddModelError("Image", "Image formatinda deyil");
                return View(vm);
            }
            string uniqueFileName = await vm.Image.FileUploadAsync(_folderPath);
            Trainer trainer = new()
            {
                Name = vm.Name,
                ImagePath = uniqueFileName,
                SpecialtyId = vm.SpecialtyId
            };
            await _context.Trainers.AddAsync(trainer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);

            if (trainer is null)
                return NotFound();

            TrainerUpdateVM vm = new()
            {
                Id = trainer.Id,
                Name = trainer.Name,
                SpecialtyId= trainer.SpecialtyId
            };

            await _sendSpecialtiesWithViewBag();

            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> Update(TrainerUpdateVM vm)
        {
            await _sendSpecialtiesWithViewBag();

            if (!ModelState.IsValid)
                return View(vm);

            var isExistSpecialty = await _context.Specialties.AnyAsync(x => x.Id == vm.SpecialtyId);

            if (!isExistSpecialty)
            {
                ModelState.AddModelError("SpecialtyId", "Bele Specialty yoxdur.");
                return View(vm);
            }


            if (!vm.Image?.CheckSize(2) ??false)
            {
                ModelState.AddModelError("Image", "Olcu 2mb boyukdue");
                return View(vm);
            }
            if (!vm.Image?.CheckType("image") ??false)
            {
                ModelState.AddModelError("Image", "Image formatinda deyil");
                return View(vm);
            }


            var existTrainer = await _context.Trainers.FindAsync(vm.Id);

            if (existTrainer is null)
                return BadRequest();

            existTrainer.Name = vm.Name;
            existTrainer.SpecialtyId = vm.SpecialtyId;


            if (vm.Image is { })
            {
                string newImagePath = await vm.Image.FileUploadAsync(_folderPath);

                string oldImagePath = Path.Combine(_folderPath, existTrainer.ImagePath);
                FileHelper.FileDelete(oldImagePath);

                existTrainer.ImagePath = newImagePath;
            }


            _context.Trainers.Update(existTrainer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);

            if (trainer is null)
                return NotFound();

            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();


            string deletedImagePath = Path.Combine(_folderPath, trainer.ImagePath);

            FileHelper.FileDelete(deletedImagePath);

            return RedirectToAction("Index");

        }


        private async Task _sendSpecialtiesWithViewBag()
        {
            var specialties = await _context.Specialties.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToListAsync();

            ViewBag.Specialties = specialties;
        }

    }
}
