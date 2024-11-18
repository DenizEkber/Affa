using AFFA.src.DEPO.DataBase.Configurations;
using AFFA.src.DEPO.DataBase.Entity;
using Microsoft.EntityFrameworkCore;


namespace AFFA.src.DEPO.DataBase.Context
{
    public class AFFADbContext : DbContext
    {
        private readonly string connectionString = "Server=USER\\SQLEXPRESS;Database=AFFA_Db;Trusted_Connection=True;TrustServerCertificate=True;";
        public DbSet<Bet> Bets { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchResult> MatchResults { get; set; }
        public DbSet<Standing> Standings { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BetConfiguration());
            modelBuilder.ApplyConfiguration(new ClubConfiguration());
            modelBuilder.ApplyConfiguration(new MatchConfiguration());
            modelBuilder.ApplyConfiguration(new MatchResultConfiguration());
            modelBuilder.ApplyConfiguration(new StandingConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        }
    }
}
