using WebApplication5.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Configuration;

namespace WebApplication5.Models
{
    public class WebApplication5Context : DbContext
    {
        public WebApplication5Context() : base("DefaultConnection")
        {
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        //    base.OnModelCreating(modelBuilder);
        //}

        public DbSet<CricketLeagues> cricketL{ get; set; }
        public DbSet<Event> Eventss { get; set; }
        public DbSet<CricketTeams> cricketT { get; set; }
        public DbSet<CricketPlayers> cricketPlayers { get; set; }
       public DbSet<SoccerLeagues> soccerL { get; set; }//THE ACTUAL LEAGUE
      public DbSet<SoccerTeams> soccerT { get; set; }
      public DbSet<SoccerPlayers> soccerPlayers { get; set; }
        public DbSet<VolleyballLeagues> volleyL { get; set; }
      public DbSet<LeagueRegisteration> LeagueReg { get; set; }
        public DbSet<CLeagueRegistration> CLeagueReg { get; set; }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<CourtBooking> courtBooking { get; set; }

        public DbSet<pass> passs{ get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<SoccerLeague> soccerLeague { get; set; }//ADDED TO A LEAGUE AFTER PAYMENT.
       public DbSet<Fixtures> fixtures { get; set; }
        public DbSet<SoccerResults> soccerResults { get; set; }

        //public System.Data.Entity.DbSet<WebApplication5.Models.SoccerLog> SoccerLogs { get; set; }
        public DbSet<SoccerLog> SoccerLogs { get; set; }

        public DbSet<Court1> court1 { get; set; }

        public DbSet<Court2> court2 { get; set; }

        public DbSet<Court3> court3 { get; set; }
       public DbSet<Referee> referee { get; set; }

        public DbSet<RefereeMatch> refereeMatch { get; set; }

        public DbSet<CricketFixtures> CricketFixtures { get; set; }

        public DbSet<CricketLeague> CricketLeague { get; set; }

        public DbSet<CricketResults> CricketResults { get; set; }

        public DbSet<CricketLog> CricketLogs { get; set; }

        public DbSet<umpire> umpire { get; set; }
        public DbSet<RefRating> refrating { get; set; }

        public DbSet<Umpirerate> umpirerate { get; set; }


    }
}