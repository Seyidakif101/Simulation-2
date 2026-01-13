using Simulation_2.Models.Common;

namespace Simulation_2.Models
{
    public class Trainer:BaseEntity
    {
        public string Name { get; set; }= string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public int SpecialtyId { get; set; }
        public Specialty Specialty { get; set; } = null!;
    }
}
