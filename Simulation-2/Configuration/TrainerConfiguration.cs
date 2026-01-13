using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simulation_2.Models;

namespace Simulation_2.Configuration
{
    public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
    {
        public void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
            builder.Property(t => t.ImagePath).IsRequired().HasMaxLength(1000);
        }
    }
}
