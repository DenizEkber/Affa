using AFFA.src.DEPO.DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AFFA.src.DEPO.DataBase.Configurations
{
    public class BetConfiguration : IEntityTypeConfiguration<Bet>
    {
        public void Configure(EntityTypeBuilder<Bet> builder)
        {
            builder.HasKey(b => b.BetId);


            builder.HasOne(b => b.Customer)
                   .WithMany(c => c.Bets)
                   .HasForeignKey(b => b.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(b => b.Team)
                   .WithMany()
                   .HasForeignKey(b => b.TeamId)
                   .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(b => b.Match)  
                   .WithMany()  
                   .HasForeignKey(b => b.MatchId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
