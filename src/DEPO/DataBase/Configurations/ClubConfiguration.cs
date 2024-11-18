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
    public class ClubConfiguration : IEntityTypeConfiguration<Club>
    {
        public void Configure(EntityTypeBuilder<Club> builder)
        {
            builder.HasKey(c => c.ClubId);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(c => c.Standing)
                .WithOne(s => s.Club)
                .HasForeignKey<Standing>(s => s.ClubId);
        }
    }
}
