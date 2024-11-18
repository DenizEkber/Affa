using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFFA.src.DEPO.DataBase.Entity;

namespace AFFA.src.DEPO.DataBase.Configurations
{
    public class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.HasKey(m => m.MatchId);

            builder.HasOne(m => m.HomeClub)
                .WithMany(c => c.HomeMatches)
                .HasForeignKey(m => m.HomeClubId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.AwayClub)
                .WithMany(c => c.AwayMatches)
                .HasForeignKey(m => m.AwayClubId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.MatchResult)
                .WithOne(r => r.Match)
                .HasForeignKey<MatchResult>(r => r.MatchId);
        }
    }
}
