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
    public class MatchResultConfiguration : IEntityTypeConfiguration<MatchResult>
    {
        public void Configure(EntityTypeBuilder<MatchResult> builder)
        {
            builder.HasKey(r => r.MatchResultId);

            builder.HasOne(r => r.Match)
                .WithOne(m => m.MatchResult)
                .HasForeignKey<MatchResult>(r => r.MatchId);
        }
    }

}
