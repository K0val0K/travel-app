using eTickets.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Data
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country_Tour>().HasKey(am => new
            {
                am.CountryId,
                am.TourId
            });

            modelBuilder.Entity<Country_Tour>().HasOne(m => m.Tour).WithMany(am => am.Countries_Tours).HasForeignKey(m => m.TourId);
            modelBuilder.Entity<Country_Tour>().HasOne(m => m.Country).WithMany(am => am.Countries_Tours).HasForeignKey(m => m.CountryId);


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Country_Tour> Countries_Tours { get; set; }
        public DbSet<TravelAgency> TravelAgencies { get; set; }


        //Orders related tables
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        public DbSet<UserTourBookmark> Bookmarks { get; set; }
    }
}
