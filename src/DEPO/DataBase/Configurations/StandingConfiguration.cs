using AFFA.src.DEPO.DataBase.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFFA.src.DEPO.DataBase.Configurations
{
    public class StandingConfiguration : IEntityTypeConfiguration<Standing>
    {
        public void Configure(EntityTypeBuilder<Standing> builder)
        {
            builder.HasKey(s => s.StandingId);

            builder.Property(s => s.Points)
                .IsRequired();

            builder.Property(s => s.MatchesPlayed)
                .IsRequired();

            builder.HasOne(s => s.Club)
                .WithOne(c => c.Standing)
                .HasForeignKey<Standing>(s => s.ClubId);
        }
    }
}
