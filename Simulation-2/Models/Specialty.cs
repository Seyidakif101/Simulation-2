using Simulation_2.Models.Common;

namespace Simulation_2.Models
{
    public class Specialty:BaseEntity
    {
        public string Name { get; set; }= string.Empty;
        public ICollection<Trainer> Trainers { get; set; } = [];

    }
}
