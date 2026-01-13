using System.ComponentModel.DataAnnotations;

namespace Simulation_2.ViewModels.TrainerViewModels
{
    public class TrainerUpdateVM
    {
        public int Id { get; set; }
        [Required,MaxLength(100),MinLength(3)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public IFormFile? Image { get; set; }
        [Required]
        public int SpecialtyId { get; set; }
    }
}
