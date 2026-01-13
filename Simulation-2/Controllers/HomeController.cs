using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simulation_2.Context;
using Simulation_2.ViewModels.TrainerViewModels;
using System.Diagnostics;

namespace Simulation_2.Controllers
{
    public class HomeController(AppDbContext _context) : Controller
    {
       
        public async Task<IActionResult> IndexAsync()
        {

            var trainers = await _context.Trainers.Select(x => new TrainerGetVM()
            {
                Id = x.Id,
                Name = x.Name,
                ImagePath = x.ImagePath,
                SpecialtyName = x.Specialty.Name
            }).ToListAsync();

            return View(trainers);
        }

    }
}
