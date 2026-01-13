using System.ComponentModel.DataAnnotations;

namespace Simulation_2.ViewModels.SpecialtyViewModels
{
    public class SpecialtyCreateVM
    {
        public int Id { get; set; }
        [Required,MaxLength(100),MinLength(3)]
        public string Name { get; set; } = string.Empty;
    }
}
